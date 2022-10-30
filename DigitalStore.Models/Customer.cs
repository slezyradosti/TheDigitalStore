using DigitalStore.Models.NotForDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStore.Models.Base;

namespace DigitalStore.Models
{
    public class Customer : EntityBase
    {
        [Required]
        [StringLength(25)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(25)]
        public string MidName { get; set; }
        [Required]
        [StringLength(25)]
        public string LastName { get; set; }
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Phone number must contain 10 digits")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public int CityId { get; set; }
        public virtual ICollection<AspUsersCustomer> AspUsersCustomers { get; set; }

        [ForeignKey(nameof(CityId))]
        public City City { get; set; }
    }
}
