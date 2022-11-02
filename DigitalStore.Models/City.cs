using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStore.Models.Base;

namespace DigitalStore.Models
{
    public class City: EntityBase
    {
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Customer> Customers { get; set; } = new List<Customer>();

        [StringLength(50)]
        public string CityName { get; set; }
    }
}
