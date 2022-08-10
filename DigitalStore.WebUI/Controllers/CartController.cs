using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalStore.Models;
using DigitalStore.Repos;
using ReflectionIT.Mvc.Paging;
using DigitalStore.Models.NotForDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace DigitalStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepo _repo;
        public CartController(IProductRepo repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View();
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

        public Cart GetCart()
        {
            //Cart cart = (Cart)Session["Cart"];
            //if (cart == null)
            //{
            //    cart = new Cart();
            //    Session["Cart"] = cart;
            //}
            //return cart;
        }
    }
}
