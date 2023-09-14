using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCSPro.Models
{
    public class Order
    {
        //internal int Count;

        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int OrderStatus { get; set; }
        public int OrderType { get; set; }
        public Guid OrderBy { get; set; }
        public DateTime OrderedOn { get; set; }
        public DateTime? ShippedOn { get; set; } // Nullable DateTime
        public bool IsActive { get; set; }
    }
}
