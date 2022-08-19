using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStore.Models.Base;

namespace DigitalStore.Models
{
    public class Customer : EntityBase
    {
        [StringLength(25)]
        //[Index("IDX_CreditRisk_Name", IsUnique = true, Order = 1)]
        public string FirstName { get; set; }
        [StringLength(25)]
        //[Index("IDX_CreditRisk_Name", IsUnique = true, Order = 1)]
        public string MidName { get; set; }
        [StringLength(25)]
        //[Index("IDX_CreditRisk_Name", IsUnique = true, Order = 1)]
        public string LastName { get; set; }
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        public int CityId { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();

        [ForeignKey("CityId")]
        public City City { get; set; }
    }
}