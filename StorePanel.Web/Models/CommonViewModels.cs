using Newtonsoft.Json.Linq;
using StorePanel.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Models
{
    public class CartModel
    {
        public CartModel()
        {
        }

        public CartModel(string json)
        {
            JObject jObject = JObject.Parse(json);
            var jItems = jObject["CartItems"].ToList();
            var cartItems = new List<CartItemModel>();
            foreach (var item in jItems)
            {
                cartItems.Add(new CartItemModel()
                {
                    Id = Convert.ToInt32(item["Id"]),
                    ProductName = (string)item["ProductName"],
                    Image = (string)item["Image"],
                    Price = Convert.ToInt64(item["Price"]),
                    MainFeatureId = Convert.ToInt32(item["MainFeatureId"]),
                    Quantity = Convert.ToInt32(item["Quantity"])
                });
            }

            this.CartItems = cartItems;
            this.TotalPrice = Convert.ToInt64(jObject["TotalPrice"]);
        }
        public List<CartItemModel> CartItems { get; set; }
        public long TotalPrice { get; set; }
    }
    public class CartItemModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
        public int MainFeatureId { get; set; }
    }

    public class CustomerDashboardViewModel
    {
        public Customer Customer { get; set; }
        public List<CustomerInvoiceViewModel> Invoices { get; set; }
    }
    public class CustomerInvoiceViewModel
    {
        public CustomerInvoiceViewModel()
        {

        }

        public CustomerInvoiceViewModel(Invoice invoice)
        {
            this.Id = invoice.Id;
            this.InvoiceNumber = invoice.InvoiceNumber;
            this.IsPayed = invoice.IsPayed;
            this.RegisterDate = invoice.AddedDate;
            this.Price = invoice.TotalPrice.ToString("##,###");
            this.PersianDate = new PersianDateTime(invoice.AddedDate).ToString("dddd d MMMM yyyy");
        }
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime RegisterDate { get; set; }
        public string PersianDate { get; set; }
        public string Price { get; set; }
        public bool IsPayed { get; set; }
    }
    public class RegisterCustomerViewModel
    {
        [Display(Name = "نام")]
        //[Required(ErrorMessage = "{0} را وارد کنید")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        //[Required(ErrorMessage = "{0} را وارد کنید")]
        public string LastName { get; set; }
        [Display(Name = "نام کاربری")]
        //[Required(ErrorMessage = "{0} را وارد کنید")]
        public string UserName { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست")]
        public string Email { get; set; }
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "{0} باید حداقل 6 کارکتر باشد", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
            ErrorMessage = "پسورد باید حداقل 6 کارکتر و شامل یک حرف بزرگ یک حرف کوچک یک عدد و یک کارکتر خاص باشد.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Required(ErrorMessage = "{0} را وارد کنید")]
        //[DataType(DataType.Password)]
        //[Display(Name = "تکرار رمز عبور")]
        //[Compare("Password", ErrorMessage = "عدم تطابق رمز عبور جدید و تکرار رمز عبور جدید")]
        //public string ConfirmPassword { get; set; }
    }

    public class WishListModel
    {
        public WishListModel()
        {
        }

        public WishListModel(string json)
        {
            JObject jObject = JObject.Parse(json);
            var jItems = jObject["WishListItems"].ToList();
            var wishListItems = new List<WishListItemModel>();
            foreach (var item in jItems)
            {
                wishListItems.Add(new WishListItemModel()
                {
                    Id = Convert.ToInt32(item["Id"]),
                    ProductName = (string)item["ProductName"],
                    Image = (string)item["Image"],
                });
            }

            this.WishListItems = wishListItems;
        }
        public List<WishListItemModel> WishListItems { get; set; }
    }
    public class WishListItemModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
    }

    public class CheckoutForm
    {
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Name { get; set; }
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "ایمیل نا معتبر")]
        [MaxLength(400, ErrorMessage = "{0} باید کمتر از 400 کارکتر باشد")]
        public string Email { get; set; }
        [Display(Name = "تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400, ErrorMessage = "{0} باید کمتر از 400 کارکتر باشد")]
        public string Phone { get; set; }
        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Address { get; set; }
        [Display(Name = "کد پستی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PostalCode { get; set; }
        public int GeoDivisionId { get; set; }
        [Display(Name = "پیام")]
        [DataType(DataType.MultilineText)]
        [MaxLength(800, ErrorMessage = "{0} باید کمتر از 800 کارکتر باشد")]
        public string Message { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime RegisterDate { get; set; }
    }


    public class CheckoutViewModel
    {
        public CartModel Cart { get; set; }
        public CheckoutForm Form { get; set; }
    }

    public class InvoiceDetails
    {
        public Invoice Invoice { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; }
        public string PersianRegisterDate { get; set; }
    }
}


