using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.WebUI.Controllers
{
    public class PersonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
