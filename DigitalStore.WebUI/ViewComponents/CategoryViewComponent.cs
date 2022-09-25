using System.Threading.Tasks;
using DigitalStore.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace DigitalStore.WebUI.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ICategoryRepo _repo;

        public CategoryViewComponent(ICategoryRepo repo)
        {
            _repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = _repo.GetAll().AsQueryable().OrderBy(c => c.CategoryName);
            if (categories != null)
            {
                return View("CategoryPartialView", categories);
            }
            return new ContentViewComponentResult("Unable to locate records.");
        }
    }
}
