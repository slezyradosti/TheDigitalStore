using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalStore.Models;
using ReflectionIT.Mvc.Paging;
using DigitalStore.Repos.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace DigitalStore.WebUI.Controllers
{
    [Authorize("AdminAccess")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepo _repo;
        private const int pageSize = 10;
        private const int categoriesPageSize = 15;

        public CategoryController(ICategoryRepo repo)
        {
            _repo = repo;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {   
            return View();
        }

        public IActionResult CategoryList(int pageIndex = 1)
        {
            var qry = _repo.GetAll().AsQueryable().AsNoTracking().OrderBy(c => c.CategoryName);
            var model = PagingList.Create(qry, categoriesPageSize, pageIndex);
            model.Action = nameof(CategoryList);
            return View(model);
        }

        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var category =_repo.GetOne(Id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repo.Update(category);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError(string.Empty,
                        $@"Unable to save the record. Another user has updated it. {ex.Message}");
                    return View(category);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $@"Unable to save the record. {ex.Message}");
                    return View(category);
                }
                return RedirectToAction("CategoryList");
            }
            return View(category);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var category = _repo.GetOne(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([Bind("Id,Timestamp")] Category category)
        {
            try
            {
                _repo.Delete(category);
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
            return RedirectToAction("CategoryList");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CategoryName")] Category category)
        {
            ModelState.Remove("Timestamp"); // why Bind[] is not working? or?
            if (!ModelState.IsValid) return View(category);
            try
            {
                _repo.Add(category);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $@"Unable to create record: {ex.Message}");
                return View(category);
            }
            return RedirectToAction("CategoryList");
        }
    }
}
