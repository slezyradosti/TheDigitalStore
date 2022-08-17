using System.Collections.Generic;
using System.Linq;
using DigitalStore.EF;
using DigitalStore.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.EF;

namespace DigitalStore.Repos
{
    public class CustomerRepo : BaseRepo<Customer>, ICustomerRepo
    {
        public CustomerRepo(DigitalStoreContext context) : base(context)
        {
        }

        public List<Customer> GetRelatedData()
            => Context.Customers.FromSqlInterpolated($"SELECT * FROM Customer").Include(c => c.City.CityName).ToList();
    }
}
