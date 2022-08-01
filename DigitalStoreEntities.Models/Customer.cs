using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStore.Models.Base;

namespace DigitalStore.Models
{
    internal class Customer : EntityBase
    {
        [Key]
        public int CustomerId { get; set; }
        [StringLength(25)]
        //[Index("IDX_CreditRisk_Name", IsUnique = true, Order = 1)]
        public string Firstname { get; set; }
        [StringLength(25)]
        //[Index("IDX_CreditRisk_Name", IsUnique = true, Order = 1)]
        public string Midname { get; set; }
        [StringLength(25)]
        //[Index("IDX_CreditRisk_Name", IsUnique = true, Order = 1)]
        public string LastName { get; set; }
        [StringLength(10)]
        public string PhoneNumber { get; set; }
        public int CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        public City City { get; set; }
    }
}
