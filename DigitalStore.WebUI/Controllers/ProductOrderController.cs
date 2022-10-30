using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.Identity;
using DigitalStore.Repos.Interfaces;
using ReflectionIT.Mvc.Paging;
using DigitalStore.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Data.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using DigitalStore.Models;
using System.Data.Entity.Infrastructure;

namespace DigitalStore.WebUI.Controllers
{
    [Authorize("AdminAccess")]
    public class ProductOrderController : Controller
    {
        private readonly IProductOrderRepo _productOrderRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly IOrderRepo _orderRepo;
        private readonly IProductOrderLogic _productOrderLogic;
        private readonly IProductRepo _productRepo;
        private const int _categoriesPageSize = 10;
        private const int _productOrderPageSize = 15;

        public ProductOrderController(ICustomerRepo customerRepo, IOrderRepo orderRepo, 
            IProductOrderRepo productOrderRepo,
            IProductOrderLogic productOrderLogic,
            IProductRepo productRepo)
        {
            _customerRepo = customerRepo;
            _orderRepo = orderRepo;
            _productOrderRepo = productOrderRepo;
            _productOrderLogic = productOrderLogic;
            _productRepo = productRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult UserOrderList(int pageIndex = 1)
        {
            var userId = User.Identity.GetUserId();
            var customers = _customerRepo.GetCustomerIdByUserId(userId);

            var productOrders = _productOrderLogic.GetOrdersOfCustomers(customers);
            ViewBag.Sum = _productOrderLogic.FindSumOfAllOrders(productOrders);

            var model = PagingList.Create(productOrders, _categoriesPageSize, pageIndex);
            model.Action = nameof(UserOrderList);
            return View(model);
        }

        public IActionResult ProductOrderList(int pageIndex = 1)
        {
            var qry = _productOrderRepo.GetRelatedData().AsQueryable().AsNoTracking().OrderBy(c => c.Id);
            var model = PagingList.Create(qry, _productOrderPageSize, pageIndex);
            model.Action = nameof(ProductOrderList);
            return View(model);
        }

        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var customer = _productOrderRepo.GetOne(Id);
            if (customer == null)
            {
                return NotFound();
            }

            SetCustomersAndCitiesToViewBag();
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductOrder productOrder)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _productOrderRepo.Update(productOrder);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError(string.Empty,
                        $@"Unable to save the record. Another user has updated it. {ex.Message}");
                    return View(productOrder);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $@"Unable to save the record. {ex.Message}");
                    return View(productOrder);
                }
                return RedirectToAction("CustomerList");
            }
            return View(productOrder);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var customer = _productOrderRepo.GetOne(id);
            if (customer == null)
            {
                return NotFound();
            }

            SetCustomersAndCitiesToViewBag();
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([Bind("Id,Timestamp")] ProductOrder productOrder)
        {
            try
            {
                _productOrderRepo.Delete(productOrder);
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
            return RedirectToAction("CustomerList");
        }

        public IActionResult Create()
        {
            SetCustomersAndCitiesToViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductOrder productOrder)
        {
            ModelState.Remove("Timestamp");
            if (!ModelState.IsValid) return View(productOrder);
            try
            {
                _productOrderRepo.Add(productOrder);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $@"Unable to create record: {ex.Message}");
                SetCustomersAndCitiesToViewBag();
                return View(productOrder);
            }
            return RedirectToAction("CustomerList");
        }

        private void SetCustomersAndCitiesToViewBag()
        {
            ViewBag.products = new SelectList(_productRepo.GetAll(), "Id", "ProductName");
            ViewBag.orders = new SelectList(_orderRepo.GetAll(), "Id", "OrderDate");
        }
    }
}
