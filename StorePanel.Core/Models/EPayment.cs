using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StorePanel.Core.Models
{
    public class EPayment : BaseEntity
    {
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public long Amount { get; set; }
        [MaxLength(2000)]
        public string Description { get; set; }
        [MaxLength(255)]
        public string ExtraInfo { get; set; }
        public string RetrievalRefNo { get; set; }
        public string SystemTraceNo { get; set; }
        public string Token { get; set; }
        public string Url { get; set; }
        public string PaymentKey { get; set; }
        public string Title { get; set; }
        public int PaymentAccountId { get; set; }
        public PaymentAccount PaymentAccount { get; set; }
        public ICollection<EPaymentLog> EPaymentLogs { get; set; }
    }
}
