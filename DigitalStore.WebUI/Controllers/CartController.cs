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
        private static Cart Cart = new Cart();
        
        public CartController(IProductRepo repo)
        {
            _repo = repo;
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
                GetCart().AddItem(product, 1);
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int Id, string returnUrl)
        {
            Product product = _repo.GetOne(Id);

            if (product != null)
            {
                GetCart().RemoveLine(product);
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
        public Cart GetCart()
        {
            return Cart;
        }
    }
}
