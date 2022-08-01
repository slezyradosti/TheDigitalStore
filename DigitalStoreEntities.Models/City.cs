using System.ComponentModel.DataAnnotations;
using DigitalStore.Models.Base;

namespace DigitalStore.Models
{
    public class City : EntityBase
    {
        [Key]
        public int CityId { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Customer> Customers { get; set; } = new List<Customer>();

        [StringLength(50)]
        //[Index("IDX_CreditRisk_Name", IsUnique = true, Order = 1)]
        public string CityName { get; set; }
    }
}
