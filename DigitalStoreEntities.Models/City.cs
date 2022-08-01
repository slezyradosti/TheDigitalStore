using System.ComponentModel.DataAnnotations;
using DigitalStore.Models.Base;

namespace DigitalStore.Models
{
    internal class City : EntityBase
    {
        [Key]
        public int CityId { get; set; }

        [StringLength(50)]
        //[Index("IDX_CreditRisk_Name", IsUnique = true, Order = 1)]
        public string CityName { get; set; }
    }
}
