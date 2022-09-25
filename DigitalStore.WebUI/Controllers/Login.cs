using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.WebUI.Controllers
{
    public class Login : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
