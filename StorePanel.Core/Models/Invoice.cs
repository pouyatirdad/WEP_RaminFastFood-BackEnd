using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Core.Models
{
    public class Invoice : BaseEntity
    {
        [DisplayName("قیمت")]
        public long TotalPrice { get; set; }
        [DisplayName("تاریخ ثبت")]
        public DateTime AddedDate { get; set; }
        [DisplayName("شماره سفارش")]
        public string InvoiceNumber { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        [DisplayName("نام مشتری")]
        [MaxLength(500,ErrorMessage = "نام وارد شده باید از 500 کارکتر کمتر باشد")]
        public string CustomerName { get; set; }
        public ICollection<InvoiceItem> InvoiceItems { get; set; }
        public int? GeoDivisionId { get; set; }
        public GeoDivision GeoDivision { get; set; }
        [MaxLength(500)]
        [DisplayName("آدرس")]
        public string Address { get; set; }
        [MaxLength(50)]
        [DisplayName("شماره تلفن")]
        public string Phone { get; set; }
        [DisplayName("پرداخت شده")]
        public bool IsPayed { get; set; }
        //public ICollection<EPayment> EPayments { get; set; }

        [DisplayName("ایمیل")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("کد پستی")]
        [Required]
        public string PostalCode { get; set; }

    }
}
