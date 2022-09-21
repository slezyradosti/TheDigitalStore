using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DigitalStore.Models;
using DigitalStore.Repos;
using DigitalStore.Models.NotForDB;
using DigitalStore.WebUI.ExtensionClasses;

namespace DigitalStore.WebUI.Controllers
{
	public class CartController : Controller
    {
        private readonly IProductRepo _productRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly IOrderProcessor _orderProcessor;
        private readonly ICityRepo _cityRepo;
        //private static Cart Cart = new Cart();

        public CartController(IProductRepo prepo, ICustomerRepo crepo, ICityRepo cityRepo, IOrderProcessor processor)
        {
            _productRepo = prepo;
            _customerRepo = crepo;
            _orderProcessor = processor;
            _cityRepo = cityRepo;
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
            return RedirectToAction("Index");
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
            return RedirectToAction("Index", new { returnUrl });
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
        public IActionResult Checkout([Bind("FirstName, MidName, LastName, PhoneNumber, EMail, CityId")] Customer customer)
        {
            var cart = GetCart();
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your basket is empty!");
            }

            customer.City = _cityRepo.GetOne(customer.CityId);

            //if (ModelState.IsValid) // obj City and Timestamp Invalid. WHY????????
            //{
            //добавляю неавторизованного покупателя
            _customerRepo.Add(customer);

            _orderProcessor.SendPurchaseEmailAsync(customer, "Your Purchase", GetCart());
            var order = _orderProcessor.CreateOrder(customer);
            _orderProcessor.AddOrderListToDb(GetCart(), order);

            ClearCart(cart);
            TempData["success"] = "Order processed";
            return View("Completed");
            //}
            //else
            //{
            //    ViewBag.cities = new SelectList(_cityRepo.GetAll(), "Id", "CityName");
            //    return View(customer);
            //}
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
        public IActionResult Buy(Customer customer)
        {
            var cart = GetCart();
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your basket is empty!");
            }
            customer.City = _cityRepo.GetOne(customer.CityId);

            _customerRepo.Add(customer);

            _orderProcessor.SendPurchaseEmailAsync(customer, "Your Purchase", GetCart());
            var order = _orderProcessor.CreateOrder(customer);
            _orderProcessor.AddOrderListToDb(GetCart(), order);

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
            cart.AddItem(product, 1);
            SetCart(cart);
        }
        public void ClearCart(Cart cart)
        {
            cart.Clear();
            SetCart(cart);
        }

    }
}
