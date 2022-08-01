using System.ComponentModel.DataAnnotations;
using DigitalStore.Models.Base;

namespace DigitalStore.Models
{
    public class Category : EntityBase
    {
        [Key]
        public int CategoryId { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();

        [StringLength(50)]
        //[Index("IDX_CreditRisk_Name", IsUnique = true, Order = 1)]
        public string CategoryName { get; set; }
    }
}
