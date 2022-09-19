using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalStore.Models;
using DigitalStore.Repos;
using ReflectionIT.Mvc.Paging;


namespace DigitalStore.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepo _repo;
        private const int pageSize = 10;

        public CategoryController(ICategoryRepo repo)
        {
            _repo = repo;
        }

        public IActionResult Index(int pageIndex = 1)
        {
            var qry = _repo.GetAll().AsQueryable().AsNoTracking().OrderBy(c => c.CategoryName);
            var model = PagingList.Create(qry, pageSize, pageIndex);
            return View(model);
        }

        public IActionResult CategoryList(int pageIndex = 1)
        {
            int categoriesPageSize = 20;
            var qry = _repo.GetAll().AsQueryable().AsNoTracking().OrderBy(c => c.CategoryName);
            var model = PagingList.Create(qry, categoriesPageSize, pageIndex);
            return PartialView("_CategoryListPartialView", model);
        }
    }
}
