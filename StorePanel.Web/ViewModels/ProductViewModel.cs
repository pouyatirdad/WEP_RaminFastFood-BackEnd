﻿using StorePanel.Core.Models;
using StorePanel.Infrastructure.Dtos.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.ViewModels
{

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
    public class ProductCommentWithPersianDateViewModel : ProductComment
    {
        public ProductCommentWithPersianDateViewModel()
        {
        }
        public ProductCommentWithPersianDateViewModel(ProductComment comment)
        {
            this.Comment = comment;
            this.PersianDate = comment.AddedDate != null ? (comment.AddedDate.Value).ToString() : "-";
        }
        public ProductComment Comment { get; set; }
        [Display(Name = "تاریخ ثبت")]
        public string PersianDate { get; set; }
    }

    public class ProductWithPriceViewModel
    {
        public ProductWithPriceViewModel()
        {
        }

        public ProductWithPriceViewModel(ProductWithPriceDto dto)
        {
            this.Id = dto.Id;
            this.Image = dto.Image;
            this.ProductGroupID = dto.ProductGroupId;
            this.ProductGroup = dto.ProductGroupName;
            this.MainFeatureId = dto.MainFeatureId;
            this.ShortTitle = dto.ShortTitle;
            this.Price = dto.Price;
            this.PriceAfterDiscount = dto.PriceAfterDiscount;
        }

        public int Id { get; set; }
        public string Image { get; set; }
        public int ProductGroupID { get; set; }
        public string ProductGroup { get; set; }
        public int MainFeatureId { get; set; }
        public string ShortTitle { get; set; }
        public long Price { get; set; }
        public long PriceAfterDiscount { get; set; }
    }

    public class GridViewModel
    {
        public int? categoryId { get; set; }
        public string searchString { get; set; }
        public long? priceFrom { get; set; }
        public long? priceTo { get; set; }
        public string brands { get; set; }
        public string subFeatures { get; set; }
        public int pageNumber { get; set; }
        public int take { get; set; }
        public string sort { get; set; }
    }
    public class ProductCommentViewModel
    {
        public ProductCommentViewModel()
        {

        }

        public ProductCommentViewModel(ProductComment comment)
        {
            this.Id = comment.Id;
            this.ParentId = comment.ParentId;
            this.Name = comment.Name;
            this.Email = comment.Email;
            this.Message = comment.Message;
            this.AddedDate = comment.AddedDate != null ? (comment.AddedDate.Value).ToString("dddd d MMMM yyyy") : "-";
        }
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string AddedDate { get; set; }
    }
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public List<ProductGallery> ProductGalleries { get; set; }
        public List<ProductMainFeature> ProductMainFeatures { get; set; }
        public List<ProductFeatureValue> ProductFeatureValues { get; set; }
        public long Price { get; set; }
        public long PriceAfterDiscount { get; set; }
        public int DiscountPercentage { get; set; }
        public List<ProductCommentViewModel> ProductComments { get; set; }
        //public CommentFormViewModel CommentForm { get; set; }
    }


}
