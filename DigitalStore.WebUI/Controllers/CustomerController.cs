using DigitalStore.Models;
using DigitalStore.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DigitalStore.WebUI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepo _repo;
        private const int customerPageSize = 15;

        public CustomerController(ICustomerRepo repo)
        {
            _repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CustomerList(int pageIndex = 1)
        {
            var qry = _repo.GetRelatedData().AsQueryable().AsNoTracking().OrderBy(c => c.FirstName);
            var model = PagingList.Create(qry, customerPageSize, pageIndex);
            model.Action = nameof(CustomerList);
            return View(model);
        }

        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var customer = _repo.GetOne(Id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repo.Update(customer);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError(string.Empty,
                        $@"Unable to save the record. Another user has updated it. {ex.Message}");
                    return View(customer);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $@"Unable to save the record. {ex.Message}");
                    return View(customer);
                }
                return RedirectToAction("CategoryList");
            }
            return View(customer);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var customer = _repo.GetOne(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([Bind("Id,Timestamp")] Customer customer)
        {
            try
            {
                _repo.Delete(customer);
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
        public IActionResult Create(Customer customer)
        {
            ModelState.Remove("Timestamp");
            if (!ModelState.IsValid) return View(customer);
            try
            {
                _repo.Add(customer);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $@"Unable to create record: {ex.Message}");
                return View(customer);
            }
            return RedirectToAction("CategoryList");
        }
    }
}
