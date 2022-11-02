using System;
using System.Collections.Generic;

namespace DigitalStore.Models
{
    public partial class AspUsersCustomer
    {
        public long Id { get; set; }
        public string UserId { get; set; } = null!;
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = null!;
        public virtual AspNetUser User { get; set; } = null!;
    }
}
