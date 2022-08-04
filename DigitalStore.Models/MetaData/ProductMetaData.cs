using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models.MetaData
{
    public class ProductMetaData
    {
        [StringLength(50)]
        [Display(Name = "Product Name")]
        public string ProductName;

        [Display(Name = "Product Price")]
        public int ProductPrice;
    }
}
