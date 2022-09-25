using System.Collections.Generic;
using System.Linq;
using DigitalStore.EF;
using DigitalStore.Models;
using DigitalStore.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.EF;

namespace DigitalStore.Repos
{
    public class ProductOrderRepo : BaseRepo<ProductOrder>, IProductOrderRepo
    {
        public ProductOrderRepo(DigitalStoreContext context) : base(context)
        {
        }

        public List<ProductOrder> GetRelatedData()
            => Context.ProductOrders.FromSqlInterpolated($"SELECT * FROM ProductOrder")
            .Include(p => p.Product.ProductName + p.Product.ProductPrice + p.Product.ProductDescription)
            .ToList();
    }
}
