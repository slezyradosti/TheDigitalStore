using System.Collections.Generic;
using System.Linq;
using DigitalStore.EF;
using DigitalStore.Models;
using DigitalStore.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.EF;

namespace DigitalStore.Repos
{
    public class CustomerRepo : BaseRepo<Customer>, ICustomerRepo
    {
        public CustomerRepo(DigitalStoreContext context) : base(context)
        {
        }
        public override List<Customer> GetAll()
            => GetAll(c => c.FirstName, true).ToList();

        public List<Customer> GetRelatedData()
            => Context.Customers.FromSqlInterpolated($"SELECT * FROM Customers")
            .Include(o => o.Orders)
            .Include(c => c.City)
            .ToList();

        public List<Customer> GetCustomerIdByUserId(string UserId)
            => Context.Customers.FromSqlInterpolated($"SELECT * FROM Customers")
            .Include(u => u.AspUsersCustomers.Where(u => u.UserId == UserId))
            .ToList();
    }
}
