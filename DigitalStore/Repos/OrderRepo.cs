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
            => Context.Orders.FromSqlInterpolated($"SELECT * FROM Order")
            .Include(o => o.Customer.FirstName + o.Customer.LastName).ToList();
    }
}
