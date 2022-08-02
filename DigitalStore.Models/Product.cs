using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStore.Models.Base;

namespace DigitalStore.Models
{
    public class Product : EntityBase
    {
        [StringLength(50)]
        //[Index("IDX_CreditRisk_Name", IsUnique = true, Order = 1)]
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public int CategoryId { get; set; }
        public List<ProductOrder> ProductOrders { get; set; } = new List<ProductOrder>();

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
    }
}