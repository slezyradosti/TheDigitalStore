using System.Collections.Generic;
using System.Linq;
using DigitalStore.EF;
using DigitalStore.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.EF;

namespace DigitalStore.Repos
{
    public class ProductRepo : BaseRepo<Product>, IProductRepo
    {
        public ProductRepo(DigitalStoreContext context) : base(context)
        {
        }

        public override List<Product> GetAll()
            => GetAll(p => p.ProductName, true).ToList();

        public List<Product> Search(string searchString)
            => Context.Products.Where(p => Functions.Like(p.ProductName, $"%{searchString}%")).ToList();
        public List<Product> Search(int? categoryId)
            => Context.Products.Where(p => p.Category.Id == categoryId).ToList();
        public List<Product> Search(Category category)
            => Context.Products.Where(p => p.Category == category).ToList();

        //public List<Product> GetPromotionalProducts()
        //    => GetSome(x => x.isPromoted == true);

        public List<Product> GetRelatedData()
            => Context.Products.FromSqlInterpolated($"SELECT * FROM Product").Include(p => p.Category.CategoryName).ToList();
    }
}
