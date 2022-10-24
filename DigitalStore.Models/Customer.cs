using DigitalStore.Models.NotForDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStore.Models.Base;

namespace DigitalStore.Models
{
    public partial class Customer : EntityBase
    {
        public Customer()
        {
            AspUsersCustomers = new HashSet<AspUsersCustomer>();
            Orders = new HashSet<Order>();
        }
        public string FirstName { get; set; } = null!;
        public string MidName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int CityId { get; set; }

        public virtual City City { get; set; } = null!;
        public virtual ICollection<AspUsersCustomer> AspUsersCustomers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
