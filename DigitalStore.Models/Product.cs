using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStore.Models.Base;

namespace DigitalStore.Models
{
    public partial class Product : EntityBase
    {
        public Product()
        {
            ProductOrders = new HashSet<ProductOrder>();
        }
        public string ProductName { get; set; } = null!;
        public int ProductPrice { get; set; }
        public int CategoryId { get; set; }
        public string ProductDescription { get; set; } = null!;
        public byte[] ProductImage { get; set; } = null!;

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
