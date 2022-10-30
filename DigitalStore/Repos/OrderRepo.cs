using System.Collections.Generic;
using System.Linq;
using DigitalStore.EF;
using DigitalStore.Models;
using DigitalStore.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.EF;

namespace DigitalStore.Repos
{
    public class OrderRepo : BaseRepo<Order>, IOrderRepo
    {
        public List<Order> GetRelatedData()
            => Context.Orders.FromSqlInterpolated($"SELECT * FROM Orders")
            .Include(c => c.Customer)
            .Include(c => c.City)
            .ToList();

        public List<Order> GetCustomerOrdersList(int customerId)
            => Context.Orders.FromSqlInterpolated($"SELECT * FROM Orders")
            .Where(o => o.CustomerId == customerId)
            .ToList();
    }
}
