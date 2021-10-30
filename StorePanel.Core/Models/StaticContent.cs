using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StorePanel.Core.Models
{
    public class StaticContent : BaseEntity
    {
        [Display(Name = "عنوان")]
        public string BannerTitle { get; set; }
        [Display(Name = "عنوان فرعی")]
        public string BannerSubTitle { get; set; }
        [Display(Name = "توضیح")]
        public string BannerDescription { get; set; }
        [Display(Name = "عنوان")]
        public string AboutUsTitle { get; set; }
        [Display(Name = "عنوان فرعی")]
        public string AboutUsSubTitle { get; set; }
        [Display(Name = "توضیح")]
        public string AboutUsDescription { get; set; }
        [Display(Name = "عنوان")]
        public string OurServicesTitle { get; set; }
        [Display(Name = "توضیح")]
        public string OurServicesDescription { get; set; }
        [Display(Name = "عنوان")]
        public string ConsultationTitle { get; set; }
        [Display(Name = "عنوان")]
        public string ContactUsTitle { get; set; }
        [Display(Name = "توضیح")]
        public string ContactUsDescription { get; set; }
        [Display(Name = "آدرس")]
        public string Address { get; set; }
        [Display(Name = "تلفن ثابت وب سایت")]
        public string Phone { get; set; }
        [Display(Name = "تلفن همراه وب سایت")]
        public string Mobile { get; set; }
        [Display(Name = "اینستاگرام")]
        public string Instagram { get; set; }
        [Display(Name = "تلگرام")]
        public string Telegram { get; set; }
        [Display(Name = "واتس اپ")]
        public string WhatsApp { get; set; }
        [Display(Name = "توییتر")]
        public string Twitter { get; set; }
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [Display(Name = "نقشه")]
        public string Map { get; set; }



        [Display(Name = "عنوان ویژگی 1")]
        public string Feature1Title { get; set; }

        [Display(Name = "توضیحات ویژگی 1")]
        public string Feature1shortDescription { get; set; }

        [Display(Name = "عنوان ویژگی 2")]
        public string Feature2Title { get; set; }

        [Display(Name = "توضیحات ویژگی 2")]
        public string Feature2shortDescription { get; set; }

        [Display(Name = "عنوان ویژگی 3")]
        public string Feature3Title { get; set; }

        [Display(Name = "توضیحات ویژگی 3")]
        public string Feature3shortDescription { get; set; }


        [Display(Name = "عنوان ویژگی 4")]
        public string Feature4Title { get; set; }

        [Display(Name = "توضیحات ویژگی 4")]
        public string Feature4shortDescription { get; set; }

        [Display(Name = "عدد شمارنده 1")]
        public string Counter1Num { get; set; }

        [Display(Name = "عنوان شمارنده 1")]

        public string Counter1Title { get; set; }

        [Display(Name = "عدد شمارنده 2")]
        public string Counter2Num { get; set; }

        [Display(Name = "عنوان شمارنده 2")]

        public string Counter2Title { get; set; }

        [Display(Name = "عدد شمارنده 3")]

        public string Counter3Num { get; set; }

        [Display(Name = "عنوان شمارنده 3")]

        public string Counter3Title { get; set; }

        [Display(Name = "عدد شمارنده 4")]
        public string Counter4Num { get; set; }

        [Display(Name = "عنوان شمارنده 4")]

        public string Counter4Title { get; set; }
    }
}
