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
            => Context.ProductOrders.FromSqlInterpolated($"SELECT * FROM ProductOrders")
            .Include(p => p.Product)
            .Include(o => o.Order).ThenInclude(c => c.Customer).ThenInclude(c => c.City)
            .ToList();

        public List<ProductOrder> GetUserOrdersList(int customerId)
            => Context.ProductOrders.FromSqlInterpolated($"SELECT * FROM ProductOrders")
            .Include(p => p.Product).ThenInclude(p => p.Category)
            .Include(o => o.Order)
            .Where(c => c.Order.Customer.Id == customerId)
            .ToList();
    }
}
