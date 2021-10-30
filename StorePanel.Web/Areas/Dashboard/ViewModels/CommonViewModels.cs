using StorePanel.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Areas.Dashboard.ViewModels
{
    public class HeaderViewModel
    {
        public string UserId { get; set; }
        public string UserInitials { get; set; }
        public string UserAvatar { get; set; }
    }
    public class NavigationMenuViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentMenuId { get; set; }
        public string Icon { get; set; }
        public string ElementIdentifier { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public bool Permitted { get; set; }

        public int? DisplayOrder { get; set; }

        public bool Visible { get; set; }
    }

    public class DiscountFormViewModel
    {
        [DisplayName("عنوان تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} باید کمتر از 500 کارکتر باشد")]
        public string Title { get; set; }
        public int DiscountType { get; set; }
        [DisplayName("میزان تخفیف")]
        [Required(ErrorMessage = "لطفا میزان تخفیف را وارد کنید")]
        public long Amount { get; set; }
        public List<int> BrandIds { get; set; }
        public List<int> ProductIds { get; set; }
        public List<int> ProductGroupIds { get; set; }
        public bool IsOffer { get; set; }
        public int? OfferId { get; set; }

        // Edit Props
        public string GroupIdentifier { get; set; }
        public List<Discount> PreviousDiscounts { get; set; }
    }

    public class CustomerGridViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Phonenumber { get; set; }
    }

    public class NewProductGroupViewModel
    {
        public string Title { get; set; }
        public List<int> BrandIds { get; set; }
        public int ParentGroupId { get; set; }
        public List<int> ProductGroupFeatureIds { get; set; }

        public string Image { get; set; }
    }

    public class NewProductViewModel
    {
        public int? ProductId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int Brand { get; set; }
        public int Rate { get; set; }
        public int ProductGroup { get; set; }
        public List<ProductFeaturesViewModel> ProductFeatures { get; set; }

    }

    public class ProductFeaturesViewModel
    {
        public int? ProductId { get; set; }
        public int FeatureId { get; set; }
        public int? SubFeatureId { get; set; }
        public string Value { get; set; }
        public bool IsMain { get; set; }
        public int? Quantity { get; set; }
        public long? Price { get; set; }
    }
    public class UpdateProductGroupViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<int> BrandIds { get; set; }
        public int ParentGroupId { get; set; }
        public List<int> ProductGroupFeatureIds { get; set; }
    }

    public class FeaturesObjViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
    public class BrandsObjViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class SubFeaturesObjViewModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
