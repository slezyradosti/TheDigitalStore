using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStore.Models.Base;

namespace DigitalStore.Models
{
    public class Category : EntityBase
    {
        public List<Product> Products { get; set; } = new List<Product>();

        [StringLength(50)]
        public string CategoryName { get; set; }
    }
}
