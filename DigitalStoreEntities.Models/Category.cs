﻿using System.ComponentModel.DataAnnotations;
using DigitalStore.Models.Base;

namespace DigitalStore.Models
{
    internal class Category : EntityBase
    {
        [Key]
        public int CategoryId { get; set; }

        [StringLength(50)]
        //[Index("IDX_CreditRisk_Name", IsUnique = true, Order = 1)]
        public string CategoryName { get; set; }
    }
}
