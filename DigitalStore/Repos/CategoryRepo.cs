using System.Collections.Generic;
using System.Linq;
using DigitalStore.EF;
using DigitalStore.Models;
using DigitalStore.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.EF;

namespace DigitalStore.Repos
{
    public class CategoryRepo : BaseRepo<Category>, ICategoryRepo
    {
        public CategoryRepo(DigitalStoreContext context) : base(context)
        {
        }

        public override List<Category> GetAll()
            => GetAll(c => c.CategoryName, true).ToList();

        public List<Category> Search(string searchString)
            => Context.Categories.Where(c => Functions.Like(c.CategoryName, $"%{searchString}%")).ToList();
    }
}
