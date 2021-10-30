using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StorePanel.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorePanel.Infrastructure.Context
{
    public class StorePanelDbContext : IdentityDbContext<User>
    {
        public StorePanelDbContext(DbContextOptions<StorePanelDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleMenuPermission>()
            .HasKey(c => new { c.RoleId, c.NavigationMenuId });

            modelBuilder.Entity<RoleMenuPermission>()
            .HasKey(c => new { c.RoleId, c.NavigationMenuId });

            modelBuilder.Entity<ArticleTag>()
            .HasKey(c => new { c.ArticleId, c.TagId });

            modelBuilder.Seed();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<SystemParameter> SystemParameters { get; set; }

        public DbSet<NavigationMenu> NavigationMenu { get; set; }

        public DbSet<RoleMenuPermission> RoleMenuPermission { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<ArticleCategory> ArticleCategories { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<ArticleTag> ArticleTags { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<ArticleComment> ArticleComments { get; set; }

        public DbSet<ContactForm> ContactUsForms { get; set; }

        public DbSet<Faq> Faqs { get; set; }

        public DbSet<FaqCategory> FaqCategories { get; set; }

        public DbSet<StaticContent> StaticContents { get; set; }

        public DbSet<PaymentAccount> PaymentAccounts { get; set; }

        public DbSet<EPayment> EPayments { get; set; }

        public DbSet<EPaymentLog> EPaymentLogs { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductComment> ProductComments { get; set; }

        public DbSet<ProductFeatureValue> ProductFeatureValues { get; set; }

        public DbSet<ProductMainFeature> ProductMainFeatures { get; set; }

        public DbSet<ProductGroup> ProductGroups { get; set; }

        public DbSet<ProductGroupBrand> ProductGroupBrands { get; set; }

        public DbSet<ProductGroupFeature> ProductGroupFeatures { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<GeoDivision> GeoDivisions { get; set; }

        public DbSet<Feature> Features { get; set; }

        public DbSet<Discount> Discounts { get; set; }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<SubFeature> SubFeatures { get; set; }

        public DbSet<ProductGallery> ProductGalleries { get; set; }

        public DbSet<Basicdefinitions> Basicdefinitions { get; set; }

        public DbSet<Slider> Sliders { get; set; }

    }
}
