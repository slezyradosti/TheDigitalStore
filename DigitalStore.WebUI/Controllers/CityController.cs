using DigitalStore.Models;
using DigitalStore.Repos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DigitalStore.WebUI.Controllers
{
    [Authorize("AdminAccess")]
    public class CityController : Controller
    {
        private readonly ICityRepo _repo;
        private const int pageSize = 10;
        private const int citiesPageSize = 15;

        public CityController(ICityRepo repo)
        {
            _repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CityList(int pageIndex = 1)
        {
            var qry = _repo.GetAll().AsQueryable().AsNoTracking().OrderBy(c => c.CityName);
            var model = PagingList.Create(qry, citiesPageSize, pageIndex);
            model.Action = nameof(CityList);
            return View(model);
        }

        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var city = _repo.GetOne(Id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(City city)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repo.Update(city);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError(string.Empty,
                        $@"Unable to save the record. Another user has updated it. {ex.Message}");
                    return View(city);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $@"Unable to save the record. {ex.Message}");
                    return View(city);
                }
                return RedirectToAction("CityList");
            }
            return View(city);
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
        public IActionResult Delete([Bind("Id,Timestamp")] City city)
        {
            try
            {
                _repo.Delete(city);
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
            return RedirectToAction("CityList");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CityName")] City city)
        {
            ModelState.Remove("Timestamp");
            if (!ModelState.IsValid) return View(city);
            try
            {
                _repo.Add(city);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $@"Unable to create record: {ex.Message}");
                return View(city);
            }
            return RedirectToAction("CityList");
        }
    }
}
