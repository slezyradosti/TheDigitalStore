using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DigitalStore.Models;
using DigitalStore.Models.NotForDB;
using DigitalStore.WebUI.ExtensionClasses;
using DigitalStore.Repos.Interfaces;
using DigitalStore.BusinessLogic.Interfaces;
using DigitalStore.BusinessLogic;

namespace DigitalStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepo _productRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly IOrderProcessor _orderProcessor;
        private readonly ICityRepo _cityRepo;
        private readonly IOrderLogic _orderLogic;
        private readonly IProductOrderLogic _productOrderLogic;

        public CartController(IProductRepo prepo, ICustomerRepo crepo, ICityRepo cityRepo, 
            IOrderProcessor processor, IOrderLogic orderLogic, IProductOrderLogic productOrderLogic)
        {
            _productRepo = prepo;
            _customerRepo = crepo;
            _orderProcessor = processor;
            _cityRepo = cityRepo;
            _orderLogic = orderLogic;
            _productOrderLogic = productOrderLogic;
        }

        public IActionResult Index(string returnUrl)
        {
            ViewBag.Title = "My Title";
            return View(GetCart());
        }

        public IActionResult AddToCart(int Id, string returnUrl)
        {
            Product product = _productRepo.GetOne(Id);

            if (product != null)
			{
				var cart = GetCart();
                AddItemToCart(cart, product);

                TempData["success"] = "Product added to Cart";
            }
            return LocalRedirect(returnUrl); 
        }

        public IActionResult RemoveFromCart(int Id, string returnUrl)
        {
            Product product = _productRepo.GetOne(Id);

            if (product != null)
            {
                var cart = GetCart();
                RemovetemToCart(cart, product);

                TempData["success"] = "Product removed from Cart";
            }
            return RedirectToAction(nameof(Index));
        }

        public Cart GetCart()
        {
            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart");
            if (cart == null)
            {
                cart = new Cart();
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return cart;
        }

        public void SetCart(Cart cart)
        {
            if (cart != null)
            { 
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
        }

        public IActionResult Checkout()
        {
            ViewBag.cities = new SelectList(_cityRepo.GetAll(), "Id", "CityName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout([Bind("FirstName, MidName, LastName, PhoneNumber, Email, CityId")] Customer customer)
        {
            var cart = GetCart();
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your basket is empty!");
            }

            //if (ModelState.IsValid) // obj City and Timestamp Invalid. why?
            //{
            //добавляю неавторизованного покупателя
            customer.City = _cityRepo.GetOne(customer.CityId);
            _customerRepo.Add(customer);

            _orderProcessor.SendPurchaseEmailAsync(customer, "Your Purchase", GetCart());
            var order = _orderLogic.CreateOrder(customer);
            _productOrderLogic.AddOrderListToDb(GetCart(), order);

            ClearCart(cart);
            TempData["success"] = "Order processed";
            return View("Completed");
        }

        public IActionResult Buy(int Id)
        {
            Product product = _productRepo.GetOne(Id);

            if (product != null)
            {
                var cart = GetCart();
                ClearCart(cart);
                AddItemToCart(cart, product);
            }

            ViewBag.cities = new SelectList(_cityRepo.GetAll(), "Id", "CityName");
            return PartialView("_BuyPartialView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Buy([Bind("FirstName, MidName, LastName, PhoneNumber, Email, CityId")] Customer customer)
        {
            var cart = GetCart();
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your basket is empty!");
            }
            customer.City = _cityRepo.GetOne(customer.CityId);

            _customerRepo.Add(customer);

            _orderProcessor.SendPurchaseEmailAsync(customer, "Your Purchase", GetCart());
            var order = _orderLogic.CreateOrder(customer);
            _productOrderLogic.AddOrderListToDb(GetCart(), order);

            ClearCart(cart);
            TempData["success"] = "Order processed";
            return View("Completed");
        }

        //for minimized code (better?)
        public void AddItemToCart(Cart cart, Product product)
		{
            cart.AddItem(product, 1);
            SetCart(cart);
        }
        public void RemovetemToCart(Cart cart, Product product)
        {
            cart.RemoveLine(product);
            SetCart(cart);
        }
        public void ClearCart(Cart cart)
        {
            cart.Clear();
            SetCart(cart);
        }
    }
}
