using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DigitalStore.Models;
using DigitalStore.Repos;
using DigitalStore.Models.NotForDB;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace DigitalStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepo _productRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly IOrderProcessor _orderProcessor;
        private readonly ICityRepo _cityRepo;
        private static Cart Cart = new Cart();
        
        public CartController(IProductRepo prepo, ICustomerRepo crepo, ICityRepo cityRepo,IOrderProcessor processor)
        {
            _productRepo = prepo;
            _customerRepo = crepo;
            _orderProcessor = processor;
            _cityRepo = cityRepo;
        }

        public IActionResult Index(string returnUrl)
        {
            //return View(new CartIndexViewModel
            //{
            //    Cart = GetCart(),
            //    ReturnUrl = returnUrl
            //});
            return View(Cart);
        }

        public IActionResult AddToCart(int Id, string returnUrl)
        {
            Product product = _productRepo.GetOne(Id);

            if (product != null)
            {
                Cart.AddItem(product, 1); // Cart = GetCart()
                TempData["success"] = "Product added to Cart";
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int Id, string returnUrl)
        {
            Product product = _productRepo.GetOne(Id);

            if (product != null)
            {
                Cart.RemoveLine(product); // Cart = GetCart()
                TempData["success"] = "Product removed from Cart";
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        //public byte[] GetCart()
        //{
        //    HttpContext.Session.TryGetValue("Cart", out byte[] cart); //HttpContext.Current.Session["Cart"];
        //    if (cart == null)
        //    {
        //        cart = new byte[1];
        //        HttpContext.Session.Set("Cart", cart);
        //    }
        //    return cart;
        //}

        public IActionResult Checkout()
        {
            ViewBag.cities = new SelectList(_cityRepo.GetAll(), "Id", "CityName");
            //ViewBag.cities = new SelectList(_cityRepo.GetOne(1));
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Checkout(Cart cart, ShippingDetails shippingDetails) // cart приходит пустой. скорее всего из-за реализации в статик, пока не знаю как решить.
        //{
        //    if (cart.Lines.Count() == 0)
        //    {
        //        ModelState.AddModelError("", "Sorry, your basket is empty!");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        _orderProcessor.ProcessOrder(cart, shippingDetails);
        //        cart.Clear();
        //        TempData["success"] = "Order processed";
        //        return View("Completed");
        //    }
        //    else
        //    {
        //        return View(shippingDetails);
        //    }
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(Customer customer)
        {
            if (Cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your basket is empty!");
            }

            customer.City = _cityRepo.GetOne(customer.CityId);

            //if (ModelState.IsValid) // obj City and Timestamp Invalid. WHY????????
            //{
            //добавляю неавторизованного покупателя
            _customerRepo.Add(customer);

            _orderProcessor.ProcessOrder(Cart, customer);
            var order = _orderProcessor.CreateOrder(customer);
            _orderProcessor.AddOrderListToDb(Cart, order);

            Cart.Clear();
            TempData["success"] = "Order processed";
            return View("Completed");
            //}
            //else
            //{
            //    ViewBag.cities = new SelectList(_cityRepo.GetAll(), "Id", "CityName");
            //    return View(customer);
            //}
        }
    }
}
