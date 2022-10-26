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

        public List<ProductOrder> GetUserOrdersList(int customerId)
            => Context.ProductOrders.FromSqlInterpolated($"SELECT * FROM ProductOrder")
            .Include(p => p.Product.ProductName + p.Product.ProductPrice)
            .Include(o => o.Order.OrderDate + o.Order.Customer.FirstName + o.Order.Customer.MidName + o.Order.Customer.PhoneNumber)
            .Where(c => c.Order.Customer.Id == customerId)
            .ToList();
    }
}
