using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStore.Models.Base;

namespace DigitalStore.Models
{
    public partial class Order : EntityBase
    {
        public Order()
        {
            ProductOrders = new HashSet<ProductOrder>();
        }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int CityId { get; set; }

        public virtual City City { get; set; } = null!;
        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
