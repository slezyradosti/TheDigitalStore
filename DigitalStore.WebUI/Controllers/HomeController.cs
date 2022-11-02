using DigitalStore.Models;
using DigitalStore.Repos.Interfaces;
using DigitalStore.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DigitalStore.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepo _repo;

        public HomeController(ILogger<HomeController> logger, IProductRepo repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> randomProductsList;
            try
            {
                randomProductsList = _repo.GetTenRandomItems(6);          
            }
            catch(Exception ex)
            {
                return View(null);
            }

            ViewBag.isHome = "home";
            return View(randomProductsList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}