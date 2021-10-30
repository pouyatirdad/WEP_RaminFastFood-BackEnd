using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Core.Models
{
    public class InvoiceItem : BaseEntity
    {
        public int Quantity { get; set; }
        public long Price { get; set; }
        public long TotalPrice { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int MainFeatureId { get; set; }
        public int? InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
