using System.Collections.Generic;
using System.Linq;
using DigitalStore.EF;
using DigitalStore.Models;
using DigitalStore.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.EF;

namespace DigitalStore.Repos
{
    public class ProductRepo : BaseRepo<Product>, IProductRepo
    {
        Random random = new Random();
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

        public List<Product> GetRelatedData()
            => Context.Products.FromSqlInterpolated($"SELECT * FROM Products").Include(p => p.Category).ToList();

        public List<Product> GetTenRandomItems(int productCount)
        {
            List<Product> newProductList = new List<Product>();

            var Ids = GetAll();
            for (int i = 0; i < productCount; i++)
            {
                var item = GetOne(Ids[random.Next(0, Ids.Count() - 1)].Id);
                if (!newProductList.Contains(item))
                {
                    newProductList.Add(item);
                }
                else
                {
                    i--;
                }
            }

            return newProductList;
        }
    }
}
