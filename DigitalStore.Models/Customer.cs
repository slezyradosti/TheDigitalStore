using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStore.Models.Base;

namespace DigitalStore.Models
{
    public class Customer : EntityBase
    {
        [Required]
        [StringLength(25)]
        //[Index("IDX_CreditRisk_Name", IsUnique = true, Order = 1)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(25)]
        //[Index("IDX_CreditRisk_Name", IsUnique = true, Order = 1)]
        public string MidName { get; set; }
        [Required]
        [StringLength(25)]
        //[Index("IDX_CreditRisk_Name", IsUnique = true, Order = 1)]
        public string LastName { get; set; }
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Phone number must contain 10 digits")]
        public string PhoneNumber { get; set; }
        [Required]
        public string EMail { get; set; }

        public int CityId { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();

        [ForeignKey("CityId")]
        [Required]
        public City City { get; set; }
    }
}