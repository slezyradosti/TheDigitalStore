using Microsoft.AspNetCore.Mvc;
using DigitalStore.Models;
using DigitalStore.Repos;
using DigitalStore.Models.NotForDB;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace DigitalStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepo _repo;
        private readonly IOrderProcessor _orderProcessor;
        private static Cart Cart = new Cart();
        
        public CartController(IProductRepo repo, IOrderProcessor processor)
        {
            _repo = repo;
            _orderProcessor = processor;
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
            Product product = _repo.GetOne(Id);

            if (product != null)
            {
                Cart.AddItem(product, 1); // Cart = GetCart()
                TempData["success"] = "Product added to Cart";
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int Id, string returnUrl)
        {
            Product product = _repo.GetOne(Id);

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
            return View(new ShippingDetails());
        }

        [HttpPost]
        public IActionResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your basket is empty!");
            }

            if (ModelState.IsValid)
            {
                _orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                TempData["success"] = "Order processed";
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }
    }
}
