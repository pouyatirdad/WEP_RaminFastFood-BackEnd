using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StorePanel.Core.Models
{
    public class EPaymentLog : BaseEntity
    {
        [MaxLength(500)]
        public string Message { get; set; }
        public DateTime LogDate { get; set; }
        [MaxLength(50)]
        public string LogType { get; set; }
        public int PaymentKey { get; set; }
        public int EPaymentId { get; set; }
        public EPayment EPayment { get; set; }
        [MaxLength(100)]
        public string MethodName { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public long Amount { get; set; }
        [MaxLength(50)]
        public string RetrievalRefNo { get; set; }
        [MaxLength(50)]
        public string StackTraceNo { get; set; }
        [MaxLength(100)]
        public string Token { get; set; }
        [MaxLength(200)]
        public string Url { get; set; }
        public string ReturnObjectSerialization { get; set; }
        [MaxLength(200)]
        public string ReturnUrl { get; set; }
        [MaxLength(500)]
        public string AdditionalData { get; set; }
        [MaxLength(10)]
        public string ResCode { get; set; }
    }
}
