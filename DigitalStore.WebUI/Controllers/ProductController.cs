using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalStore.Models;
using DigitalStore.Repos;
using ReflectionIT.Mvc.Paging;

namespace DigitalStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepo _repo;
        private const int pageSize = 5; 

        public ProductController(IProductRepo repo)
        {
            _repo = repo;
        }

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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ProductName,ProductPrice,CategoryId")] Product product)
        {
            if (!ModelState.IsValid) return View(product);
            try
            {
                _repo.Add(product);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $@"Unable to create record: {ex.Message}");
                return View(product);
            }
            return RedirectToAction(nameof(Index));
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
            return View(inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProductName,ProductPrice,CategoryId,Id,Timestamp")] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid) return View(product);
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
            return RedirectToAction(nameof(Index));
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
            int productsPageSize = 20;
            var qry = _repo.GetAll().AsQueryable().AsNoTracking().OrderBy(p => p.ProductName);
            var model = PagingList.Create(qry, productsPageSize, pageIndex);
            return View(model);
        }
    }
}
