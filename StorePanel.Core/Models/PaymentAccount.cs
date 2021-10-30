using System;
using System.Collections.Generic;
using System.Text;

namespace StorePanel.Core.Models
{
    public class PaymentAccount : BaseEntity
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string PIN { get; set; }
        public string ComebackUrl { get; set; }
        public string PaymentUrl { get; set; }
        public int MerchantId { get; set; }
        public int TerminalId { get; set; }
        public ICollection<EPayment> EPayments { get; set; }
    }
}
