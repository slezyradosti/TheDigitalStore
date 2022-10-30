using DigitalStore.Models;
using DigitalStore.Repos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReflectionIT.Mvc.Paging;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DigitalStore.WebUI.Controllers
{
    [Authorize("AdminAccess")]
    public class OrderController : Controller
    {
        private readonly IOrderRepo _orderRepo;
        private readonly ICityRepo _cityRepo;
        private readonly ICustomerRepo _customerRepo;
        private const int orderPageSize = 15;

        public OrderController(IOrderRepo orderRepo, ICityRepo cityRepo, ICustomerRepo customerRepo)
        {
            _orderRepo = orderRepo;
            _cityRepo = cityRepo;
            _customerRepo = customerRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OrderList(int pageIndex = 1)
        {
            var qry = _orderRepo.GetRelatedData().AsQueryable().AsNoTracking().OrderBy(c => c.DeliveryDate);
            var model = PagingList.Create(qry, orderPageSize, pageIndex);
            model.Action = nameof(OrderList);
            return View(model);
        }

        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var customer = _orderRepo.GetOne(Id);
            if (customer == null)
            {
                return NotFound();
            }

            SetCustomersAndCitiesToViewBag();
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _orderRepo.Update(order);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError(string.Empty,
                        $@"Unable to save the record. Another user has updated it. {ex.Message}");
                    return View(order);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $@"Unable to save the record. {ex.Message}");
                    return View(order);
                }
                return RedirectToAction("OrderList");
            }
            return View(order);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var order = _orderRepo.GetOne(id);
            if (order == null)
            {
                return NotFound();
            }

            SetCustomersAndCitiesToViewBag();
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([Bind("Id,Timestamp")] Order order)
        {
            try
            {
                _orderRepo.Delete(order);
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
            return RedirectToAction("OrderList");
        }

        public IActionResult Create()
        {
            SetCustomersAndCitiesToViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            ModelState.Remove("Timestamp");
            if (!ModelState.IsValid) return View(order);
            try
            {
                _orderRepo.Add(order);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $@"Unable to create record: {ex.Message}");
                SetCustomersAndCitiesToViewBag();
                return View(order);
            }
            return RedirectToAction("OrderList");
        }

        private void SetCustomersAndCitiesToViewBag()
        {
            ViewBag.customers = new SelectList(_customerRepo.GetAll(), "Id", "LastName");
            ViewBag.cities = new SelectList(_cityRepo.GetAll(), "Id", "CityName");
        }
    }
}
