using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStore.Models.Base;

namespace DigitalStore.Models
{
    public partial class City: EntityBase
    {
        public City()
        {
            Customers = new HashSet<Customer>();
            Orders = new HashSet<Order>();
        }
        public string CityName { get; set; } = null!;

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
