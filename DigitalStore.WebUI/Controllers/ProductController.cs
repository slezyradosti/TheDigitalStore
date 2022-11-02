using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalStore.Models;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Mvc.Rendering;
using DigitalStore.WebUI.ExtensionClasses;
using DigitalStore.Repos.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace DigitalStore.WebUI.Controllers
{
    [Authorize("AdminAccess")]
    public class ProductController : Controller
    {
        private readonly IProductRepo _repo;
        private readonly ICategoryRepo _categoryRepo;
        private const int pageSize = 5;
        private const int productsPageSize = 15;

        public ProductController(IProductRepo repo, ICategoryRepo categoryRepo)
        {
            _repo = repo;
            _categoryRepo = categoryRepo;
        }

        [AllowAnonymous]
        public IActionResult Index(int? id, string sortOrder, string searchString, int pageIndex = 1)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SearchString = searchString;

            var qry = _repo.Search(id).AsQueryable().AsNoTracking();

            if (!String.IsNullOrEmpty(searchString))
            {
                qry = qry.Where(q => q.ProductName.ToLower().Contains(searchString.ToLower())).AsQueryable();
            }

            qry = sortOrder switch
            {
                "Price descending" => qry.OrderByDescending(p => p.ProductPrice),
                _ => qry.OrderBy(p => p.ProductPrice),
            };

            var model = PagingList.Create(qry, pageSize, pageIndex);
            model.RouteValue = new RouteValueDictionary {
                { "searchString", searchString},
                { "sortOrder", sortOrder}
            };
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var product = _repo.GetOne(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Create()
        {
            ViewBag.categories = new SelectList(_categoryRepo.GetAll(), "Id", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ProductName,ProductPrice,ProductDescription,CategoryId")] Product product, IFormFile image)
        {
            try
            {
                product.ProductImage = ImageConvertor.ConvetrImageToByteArray(image);
                _repo.Add(product);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $@"Unable to create record: {ex.Message}");
                ViewBag.categories = new SelectList(_categoryRepo.GetAll(), "Id", "CategoryName");
                return View(product);
            }
            return RedirectToAction(nameof(ProductList));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var inventory = _repo.GetOne(id);
            if (inventory == null)
            {
                return NotFound();
            }
            ViewBag.categories = new SelectList(_categoryRepo.GetAll(), "Id", "CategoryName");
            return View(inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            try
            {
                _repo.Update(product);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ModelState.AddModelError(string.Empty,
                    $@"Unable to save the record. Another user has updated it. {ex.Message}");
                return View(product);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $@"Unable to save the record. {ex.Message}");
                return View(product);
            }
            return RedirectToAction(nameof(ProductList));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var inventory = _repo.GetOne(id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([Bind("Id,Timestamp")] Product product)
        {
            try
            {
                _repo.Delete(product);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ModelState.AddModelError(string.Empty,
                    $@"Unable to delete record. Another user updated the record. {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $@"Unable to create record: {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ProductList(int pageIndex = 1)
        {
            var qry = _repo.GetAll().AsQueryable().AsNoTracking().OrderBy(p => p.ProductName);
            var model = PagingList.Create(qry, productsPageSize, pageIndex);

            ViewBag.categories = new SelectList(_categoryRepo.GetAll(), "Id", "CategoryName");
            model.Action = nameof(ProductList);
            return View(model);
        }
    }
}
