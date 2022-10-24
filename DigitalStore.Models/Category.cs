using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStore.Models.Base;

namespace DigitalStore.Models
{
    public partial class Category : EntityBase
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
