using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCSPro.Models
{
    public class Supplier
    {
        public Guid SupplierId { get; set; }
        public string SupplierName { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
