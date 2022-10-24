using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DigitalStore.Models.Base;

namespace DigitalStore.Models
{
    public partial class ProductOrder : EntityBase
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Price { get; set; }
        public byte[] Timestamp { get; set; } = null!;

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
