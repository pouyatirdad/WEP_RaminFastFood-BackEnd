using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StorePanel.Core.Models;
using StorePanel.Core.Utility;
using System;

namespace StorePanel.Infrastructure.Context
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            #region Seed Users

            User admin = new User()
            {
                Id = DefaultValues.ADMIN_ID,
                Avatar = "user-avatar.png",
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "Admin",
                EmailConfirmed = true,
                NormalizedUserName = "Admin".ToUpper(),
                Email = "Admin@Admin.com",
                NormalizedEmail = "Admin@Admin.com".ToUpper()
            };
            admin.PasswordHash = GetHashedPassword(admin, "Admin");

            User superuser = new User()
            {
                Id = DefaultValues.SUPER_USER_ID,
                Avatar = "user-avatar.png",
                FirstName = "Superuser",
                LastName = "Superuser",
                UserName = "Superuser",
                EmailConfirmed = true,
                NormalizedUserName = "Superuser".ToUpper(),
                Email = "Superuser@Superuser.com",
                NormalizedEmail = "Superuser@Superuser.com".ToUpper()
            };
            superuser.PasswordHash = GetHashedPassword(superuser, "Superuser");

            modelBuilder.Entity<User>().HasData(
                admin,
                superuser
            );
            #endregion
            #region Seed Roles



            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = DefaultValues.ADMIN_ROLE_ID, Name = "Admin", NormalizedName = "Admin".ToUpper() },
                new IdentityRole { Id = DefaultValues.USER_ROLE_ID, Name = "User", NormalizedName = "User".ToUpper() },
                new IdentityRole { Id = DefaultValues.Customer_ROLE_ID, Name = "Customer", NormalizedName = "Customer".ToUpper() },
                new IdentityRole { Id = DefaultValues.SUPER_USER_ROLE_ID, Name = "Superuser", NormalizedName = "Superuser".ToUpper() }
            );
            #endregion

            #region Seed User Roles

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = DefaultValues.ADMIN_ID, RoleId = DefaultValues.ADMIN_ROLE_ID },
                new IdentityUserRole<string> { UserId = DefaultValues.SUPER_USER_ID, RoleId = DefaultValues.SUPER_USER_ROLE_ID }
            );

            #endregion

            #region Seed SystemParameter

            modelBuilder.Entity<SystemParameter>().HasData(
                new SystemParameter { Id = DefaultValues.UserDefaultPassword_sys_id, InsertUser = "SuperUser", InsertDate = DateTime.Now, Key = "DefaultPassword", Value = "User@123456" }
            );

            #endregion


            #region Seed Navigation menu
            modelBuilder.Entity<NavigationMenu>().HasData(
            #region Auth_Control

                new NavigationMenu()
                {
                    Id = 1,
                    Name = "مجوز دسترسی",
                    ElementIdentifier = "auth_control",
                    Icon = "Icon",
                    DisplayOrder = 100,
                    Visible = true,
                },

            #region Users

                new NavigationMenu()
                {
                    Id = 2,
                    ParentMenuId = 1,
                    Name = "کاربران",
                    ControllerName = "Users",
                    ActionName = "Index",
                    ElementIdentifier = "users",
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = 3,
                    ParentMenuId = 2,
                    Name = "افزودن کابر",
                    ControllerName = "Users",
                    ActionName = "Create",
                    ElementIdentifier = "users",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 4,
                    ParentMenuId = 2,
                    Name = "ویرایش کابر",
                    ControllerName = "Users",
                    ActionName = "Edit",
                    ElementIdentifier = "users",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 5,
                    ParentMenuId = 2,
                    Name = "حذف کابر",
                    ControllerName = "Users",
                    ActionName = "Delete",
                    ElementIdentifier = "users",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 6,
                    ParentMenuId = 2,
                    Name = "ویرایش نقش های کابر",
                    ControllerName = "Users",
                    ActionName = "EditRoles",
                    ElementIdentifier = "users",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 7,
                    ParentMenuId = 2,
                    Name = "ویرایش پروفایل من",
                    ControllerName = "Users",
                    ActionName = "EditMyProfile",
                    ElementIdentifier = "users",
                    Visible = false,
                },
            #endregion
            #region Roles
                new NavigationMenu()
                {
                    Id = 8,
                    ParentMenuId = 1,
                    Name = "نقش ها",
                    ControllerName = "Roles",
                    ActionName = "Index",
                    ElementIdentifier = "roles",
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = 9,
                    ParentMenuId = 8,
                    Name = "افزودن نقش",
                    ControllerName = "Roles",
                    ActionName = "Create",
                    ElementIdentifier = "roles",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 10,
                    ParentMenuId = 8,
                    Name = "ویرایش نقش",
                    ControllerName = "Roles",
                    ActionName = "Edit",
                    ElementIdentifier = "roles",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 11,
                    ParentMenuId = 8,
                    Name = "حذف نقش",
                    ControllerName = "Roles",
                    ActionName = "Delete",
                    ElementIdentifier = "roles",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 12,
                    ParentMenuId = 8,
                    Name = "ویرایش دسترسی های نقش",
                    ControllerName = "Roles",
                    ActionName = "EditRolePermission",
                    ElementIdentifier = "roles",
                    Visible = false,
                },
            #endregion

            #endregion
            #region Article Control
                new NavigationMenu()
                {
                    Id = 13,
                    Name = "مدیریت مطالب",
                    ElementIdentifier = "article_control",
                    Icon = "Icon",
                    DisplayOrder = 1,
                    Visible = true,
                },
            #region Article Category
                new NavigationMenu()
                {
                    Id = 14,
                    ParentMenuId = 13,
                    Name = "دسته بندی مطالب",
                    ControllerName = "ArticleCategory",
                    ActionName = "Index",
                    ElementIdentifier = "article_categories",
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = 15,
                    ParentMenuId = 14,
                    Name = "افزودن دسته بندی",
                    ControllerName = "ArticleCategory",
                    ActionName = "Create",
                    ElementIdentifier = "article_categories",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 16,
                    ParentMenuId = 14,
                    Name = "ویرایش دسته بندی",
                    ControllerName = "ArticleCategory",
                    ActionName = "Edit",
                    ElementIdentifier = "article_categories",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 17,
                    ParentMenuId = 14,
                    Name = "حذف دسته بندی",
                    ControllerName = "ArticleCategory",
                    ActionName = "Delete",
                    ElementIdentifier = "article_categories",
                    Visible = false,
                },
            #endregion
            #region Articles
                new NavigationMenu()
                {
                    Id = 18,
                    ParentMenuId = 13,
                    Name = "مطالب",
                    ControllerName = "Articles",
                    ActionName = "Index",
                    ElementIdentifier = "articles",
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = 19,
                    ParentMenuId = 18,
                    Name = "افزودن مطلب",
                    ControllerName = "Articles",
                    ActionName = "Create",
                    ElementIdentifier = "articles",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 20,
                    ParentMenuId = 18,
                    Name = "ویرایش مطلب",
                    ControllerName = "Articles",
                    ActionName = "Edit",
                    ElementIdentifier = "articles",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 21,
                    ParentMenuId = 18,
                    Name = "حذف مطلب",
                    ControllerName = "Articles",
                    ActionName = "Delete",
                    ElementIdentifier = "articles",
                    Visible = false,
                },
            #endregion
            #region Article Comments
                new NavigationMenu()
                {
                    Id = 22,
                    ParentMenuId = 18,
                    Name = "کامنت ها",
                    ControllerName = "ArticleComments",
                    ActionName = "Index",
                    ElementIdentifier = "articles",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 23,
                    ParentMenuId = 22,
                    Name = "افزودن کامنت",
                    ControllerName = "ArticleComments",
                    ActionName = "Create",
                    ElementIdentifier = "articles",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 24,
                    ParentMenuId = 22,
                    Name = "ویرایش کامنت",
                    ControllerName = "ArticleComments",
                    ActionName = "Edit",
                    ElementIdentifier = "articles",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 25,
                    ParentMenuId = 22,
                    Name = "حذف کامنت",
                    ControllerName = "ArticleComments",
                    ActionName = "Delete",
                    ElementIdentifier = "articles",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 26,
                    ParentMenuId = 22,
                    Name = "پاسخ دادن به کامنت",
                    ControllerName = "ArticleComments",
                    ActionName = "AnswerComment",
                    ElementIdentifier = "articles",
                    Visible = false,
                },
            #endregion
            #endregion
            #region Faq Control
                new NavigationMenu()
                {
                    Id = 27,
                    Name = "سوالات متداول",
                    ElementIdentifier = "faq_control",
                    DisplayOrder = 3,
                    Visible = true,
                },
            #region Faq Category
                new NavigationMenu()
                {
                    Id = 28,
                    ParentMenuId = 27,
                    Name = "دسته بندی سوالات متداول",
                    ElementIdentifier = "faq_categories",
                    ControllerName = "FaqCategory",
                    ActionName = "Index",
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = 29,
                    ParentMenuId = 27,
                    Name = "افزودن دسته بندی",
                    ElementIdentifier = "faq_categories",
                    ControllerName = "FaqCategory",
                    ActionName = "Create",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 30,
                    ParentMenuId = 27,
                    Name = "ویرایش دسته بندی",
                    ElementIdentifier = "faq_categories",
                    ControllerName = "FaqCategory",
                    ActionName = "Edit",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 31,
                    ParentMenuId = 27,
                    Name = "حذف دسته بندی",
                    ElementIdentifier = "faq_categories",
                    ControllerName = "FaqCategory",
                    ActionName = "Delete",
                    Visible = false,
                },
            #endregion
            #region Faqs
                new NavigationMenu()
                {
                    Id = 32,
                    ParentMenuId = 27,
                    Name = "سوالات متداول",
                    ElementIdentifier = "faqs",
                    ControllerName = "Faq",
                    ActionName = "Index",
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = 33,
                    ParentMenuId = 27,
                    Name = "افزودن سوالات متداول",
                    ElementIdentifier = "faqs",
                    ControllerName = "Faq",
                    ActionName = "Create",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 34,
                    ParentMenuId = 27,
                    Name = "ویرایش سوالات متداول",
                    ElementIdentifier = "faqs",
                    ControllerName = "Faq",
                    ActionName = "Edit",
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Id = 35,
                    ParentMenuId = 27,
                    Name = "حذف سوالات متداول",
                    ElementIdentifier = "faqs",
                    ControllerName = "Faq",
                    ActionName = "Delete",
                    Visible = false,
                },
            #endregion
            #endregion
            #region Contact Us Form
                new NavigationMenu()
                {
                    Id = 36,
                    Name = "فرم تماس با ما",
                    ElementIdentifier = "contact_us_form",
                    ControllerName = "ContactUsForm",
                    ActionName = "Index",
                    DisplayOrder = 6,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Id = 37,
                    Name = "مشاهده فرم تماس با ما",
                    ElementIdentifier = "contact_us_form",
                    ControllerName = "ContactUsForm",
                    ActionName = "Details",
                    Visible = false,
                },
            #endregion
            #region Static Content
                new NavigationMenu()
                {
                    Id = 38,
                    Name = "محتوا ثابت",
                    ElementIdentifier = "static_content",
                    ControllerName = "StaticContent",
                    ActionName = "Index",
                    Visible = true,
                    DisplayOrder = 99
                },
            #endregion
            #region Product
                            new NavigationMenu()
                            {
                                Id = 39,
                                Name = "مدیریت محصولات",
                                ElementIdentifier = "product_control",
                                Icon = "Icon",
                                DisplayOrder = 1,
                                Visible = true,
                            },
                            new NavigationMenu()
                            {
                                Id = 40,
                                ParentMenuId = 39,
                                ActionName = "Index",
                                ControllerName = "Brands",
                                Name = "برند ها",
                                ElementIdentifier = "brands",
                                Visible = true,
                            },
                            new NavigationMenu()
                            {
                                Id = 41,
                                ParentMenuId = 39,
                                ActionName = "Index",
                                ControllerName = "Features",
                                Name = "ویژگی ها",
                                ElementIdentifier = "features",
                                Visible = true,
                            },
                            new NavigationMenu()
                            {
                                Id = 42,
                                ParentMenuId = 39,
                                ActionName = "Index",
                                ControllerName = "ProductGroups",
                                Name = "دسته بندی ها",
                                ElementIdentifier = "productgroups",
                                Visible = true,
                            },
                            new NavigationMenu()
                            {
                                Id = 43,
                                ParentMenuId = 39,
                                ActionName = "Index",
                                ControllerName = "Products",
                                Name = "محصولات",
                                ElementIdentifier = "product",
                                Visible = true,
                            },
            #region Brands
                                                    new NavigationMenu()
                                                    {
                                                        Id = 44,
                                                        ParentMenuId = 39,
                                                        ActionName = "Create",
                                                        ControllerName = "Brands",
                                                        ElementIdentifier = "product",
                                                        Visible = false,
                                                    },
                                                    new NavigationMenu()
                                                    {
                                                        Id = 45,
                                                        ParentMenuId = 39,
                                                        ActionName = "Edit",
                                                        ControllerName = "Brands",
                                                        ElementIdentifier = "product",
                                                        Visible = false,
                                                    },
                                                   new NavigationMenu()
                                                   {
                                                       Id = 46,
                                                       ParentMenuId = 39,
                                                       ActionName = "Delete",
                                                       ControllerName = "Brands",
                                                       ElementIdentifier = "product",
                                                       Visible = false,
                                                   },
            #endregion


            #region Feature
                                                    new NavigationMenu()
                                                    {
                                                        Id = 47,
                                                        ParentMenuId = 39,
                                                        ActionName = "Create",
                                                        ControllerName = "Features",
                                                        ElementIdentifier = "product",
                                                        Visible = false,
                                                    },
                                                    new NavigationMenu()
                                                    {
                                                        Id = 48,
                                                        ParentMenuId = 39,
                                                        ActionName = "Edit",
                                                        ControllerName = "Features",
                                                        ElementIdentifier = "product",
                                                        Visible = false,
                                                    },
                                                   new NavigationMenu()
                                                   {
                                                       Id = 49,
                                                       ParentMenuId = 39,
                                                       ActionName = "Delete",
                                                       ControllerName = "Features",
                                                       ElementIdentifier = "product",
                                                       Visible = false,
                                                   },
            #endregion
            #region subfeature
                                                   new NavigationMenu()
                                                   {
                                                       Id = 50,
                                                       ParentMenuId = 39,
                                                       ActionName = "Index",
                                                       ControllerName = "SubFeatures",
                                                       Visible = false
                                                   },
                                                   new NavigationMenu()
                                                   {
                                                       Id = 51,
                                                       ParentMenuId = 40,
                                                       ActionName = "Create",
                                                       ControllerName = "SubFeatures",
                                                       Visible = false
                                                   },
                                                   new NavigationMenu()
                                                   {
                                                       Id = 52,
                                                       ParentMenuId = 40,
                                                       ActionName = "Delete",
                                                       ControllerName = "SubFeatures",
                                                       Visible = false
                                                   },
                                                   new NavigationMenu()
                                                   {
                                                       Id = 53,
                                                       ParentMenuId = 41,
                                                       ActionName = "Edit",
                                                       ControllerName = "SubFeatures",
                                                       Visible = false
                                                   },


            #endregion
            #region ProductGroup
                    new NavigationMenu()
                    {
                        Id = 54,
                        ParentMenuId = 42,
                        ActionName = "Index",
                        ControllerName = "ProductGroups",
                        Visible = false
                    },
                    new NavigationMenu()
                    {
                        Id = 55,
                        ParentMenuId = 42,
                        ActionName = "Create",
                        ControllerName = "ProductGroups",
                        Visible = false
                    },
                     new NavigationMenu()
                     {
                         Id = 56,
                         ParentMenuId = 42,
                         ActionName = "Edit",
                         ControllerName = "ProductGroups",
                         Visible = false
                     },
                      new NavigationMenu()
                      {
                          Id = 57,
                          ParentMenuId = 42,
                          ActionName = "Delete",
                          ControllerName = "ProductGroups",
                          Visible = false
                      },
            #endregion
            #endregion
            #region Discount
                                              new NavigationMenu()
                                              {
                                                  Id = 72,
                                                  Name = "پیشنهادات و تخفیف ها",
                                                  ElementIdentifier = "OffersDiscount_control",
                                                  Icon = "Icon",
                                                  DisplayOrder = 1,
                                                  Visible = true,
                                              },
                                  new NavigationMenu()
                                  {
                                      Id = 58,
                                      Name = "تخفیف ها",
                                      ParentMenuId = 72,
                                      ElementIdentifier = "discount_control",
                                      DisplayOrder = 4,
                                      ActionName = "Index",
                                      ControllerName = "Discount",
                                      Visible = true,
                                  },
                                                                new NavigationMenu()
                                                                {
                                                                    Id = 73,
                                                                    Name = "پیشنهادات",
                                                                    ParentMenuId = 72,
                                                                    ElementIdentifier = "offers_control",
                                                                    DisplayOrder = 5,
                                                                    ActionName = "Index",
                                                                    ControllerName = "Offers",
                                                                    Visible = true,
                                                                },
                                  new NavigationMenu()
                                  {
                                      Id = 59,
                                      ActionName = "Create",
                                      ControllerName = "Discount",
                                      ParentMenuId = 58,
                                      Visible = false,
                                      ElementIdentifier = "discount"
                                  },
                                  new NavigationMenu()
                                  {
                                      Id = 60,
                                      ActionName = "Delete",
                                      ControllerName = "Discount",
                                      ParentMenuId = 58,
                                      Visible = false,
                                      ElementIdentifier = "discount"
                                  }
                                  ,
                                  new NavigationMenu()
                                  {
                                      Id = 61,
                                      ActionName = "Edit",
                                      ControllerName = "Discount",
                                      ParentMenuId = 58,
                                      Visible = false,
                                      ElementIdentifier = "discount"
                                  },
            #endregion
            #region Offers
                                                                                  new NavigationMenu()
                                                                                  {
                                                                                      Id = 74,
                                                                                      ParentMenuId = 58,
                                                                                      ActionName = "Create",
                                                                                      ControllerName = "Offers",
                                                                                      ElementIdentifier = "offers",
                                                                                      Visible = false,
                                                                                  },
                                                    new NavigationMenu()
                                                    {
                                                        Id = 75,
                                                        ParentMenuId = 58,
                                                        ActionName = "Edit",
                                                        ControllerName = "Offers",
                                                        ElementIdentifier = "offers",
                                                        Visible = false,
                                                    },
                                                   new NavigationMenu()
                                                   {
                                                       Id = 76,
                                                       ParentMenuId = 58,
                                                       ActionName = "Delete",
                                                       ControllerName = "Offers",
                                                       ElementIdentifier = "prodoffersuct",
                                                       Visible = false,
                                                   },
            #endregion
            #region Customer
                                   new NavigationMenu()
                                   {
                                       Id = 62,
                                       Name = "مشتریان",
                                       ElementIdentifier = "customer_control",
                                       DisplayOrder = 5,
                                       ActionName = "Index",
                                       ControllerName = "Customers",
                                       Visible = true,
                                   },
                                  new NavigationMenu()
                                  {
                                      Id = 63,
                                      ActionName = "Create",
                                      ControllerName = "Customers",
                                      ParentMenuId = 62,
                                      Visible = false,
                                      ElementIdentifier = "custome"
                                  },
                                  new NavigationMenu()
                                  {
                                      Id = 64,
                                      ActionName = "Delete",
                                      ControllerName = "Customers",
                                      ParentMenuId = 62,
                                      Visible = false,
                                      ElementIdentifier = "custome"
                                  }
                                  ,
                                  new NavigationMenu()
                                  {
                                      Id = 65,
                                      ActionName = "Edit",
                                      ControllerName = "Customers",
                                      ParentMenuId = 62,
                                      Visible = false,
                                      ElementIdentifier = "customer"
                                  },
            #endregion
            #region Invoices
                                  new NavigationMenu()
                                  {
                                      Id = 66,
                                      Name = "سفارشات",
                                      ElementIdentifier = "invoices_control",
                                      DisplayOrder = 6,
                                      ActionName = "Index",
                                      ControllerName = "Invoices",
                                      Visible = true,
                                  },
                                  new NavigationMenu()
                                  {
                                      Id = 67,
                                      ActionName = "Detail",
                                      ControllerName = "Invoices",
                                      ParentMenuId = 66,
                                      Visible = false,
                                      ElementIdentifier = "invoices"
                                  },


                                  new NavigationMenu()
                                  {
                                      Id = 83,
                                      ActionName = "Edit",
                                      ControllerName = "Invoices",
                                      ParentMenuId = 66,
                                      Visible = false,
                                      ElementIdentifier = "invoices"
                                  },

                                  new NavigationMenu()
                                  {
                                      Id = 84,
                                      ActionName = "Delete",
                                      ControllerName = "Invoices",
                                      ParentMenuId = 66,
                                      Visible = false,
                                      ElementIdentifier = "invoices"
                                  },
            #endregion
            #region ProductCrud
                                                       new NavigationMenu()
                                                       {
                                                           Id = 69,
                                                           ParentMenuId = 39,
                                                           ActionName = "Create",
                                                           ControllerName = "Products",
                                                           ElementIdentifier = "product",
                                                           Visible = false,
                                                       },
                                                    new NavigationMenu()
                                                    {
                                                        Id = 70,
                                                        ParentMenuId = 39,
                                                        ActionName = "Edit",
                                                        ControllerName = "Products",
                                                        ElementIdentifier = "product",
                                                        Visible = false,
                                                    },
                                                   new NavigationMenu()
                                                   {
                                                       Id = 71,
                                                       ParentMenuId = 39,
                                                       ActionName = "Delete",
                                                       ControllerName = "Products",
                                                       ElementIdentifier = "product",
                                                       Visible = false,
                                                   },
            #endregion
            #region Static Content
                new NavigationMenu()
                {
                    Id = 77,
                    Name = "تعاریف اولیه",
                    ElementIdentifier = "Basicdefinitions",
                    ControllerName = "Basicdefinitions",
                    ActionName = "Index",
                    Visible = true,
                    DisplayOrder = 10
                },
                new NavigationMenu()
                {
                    Id = 78,
                    ParentMenuId = 77,
                    ElementIdentifier = "Basicdefinitions",
                    ControllerName = "Basicdefinitions",
                    ActionName = "Create",
                    Visible = false,
                },
            #endregion
            #region Slider
                                                  new NavigationMenu()
                                                  {
                                                      Id = 79,
                                                      Name = "اسلایدر",
                                                      ElementIdentifier = "slider_control",
                                                      DisplayOrder = 6,
                                                      ActionName = "Index",
                                                      ControllerName = "Slider",
                                                      Visible = true,
                                                  },
                                  new NavigationMenu()
                                  {
                                      Id = 80,
                                      ActionName = "Delete",
                                      ControllerName = "Slider",
                                      ParentMenuId = 79,
                                      Visible = false,
                                      ElementIdentifier = "slider"
                                  }
                                  ,
                                                                    new NavigationMenu()
                                                                    {
                                                                        Id = 81,
                                                                        ActionName = "Create",
                                                                        ControllerName = "Slider",
                                                                        ParentMenuId = 79,
                                                                        Visible = false,
                                                                        ElementIdentifier = "slider"
                                                                    }
                                  ,
                                  new NavigationMenu()
                                  {
                                      Id = 82,
                                      ActionName = "Edit",
                                      ControllerName = "Slider",
                                      ParentMenuId = 79,
                                      Visible = false,
                                      ElementIdentifier = "slider"
                                  }
                                  #endregion
            );

            #region Superuser Permissions
            modelBuilder.Entity<RoleMenuPermission>().HasData(
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 1
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 2
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 3
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 4
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 5
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 6
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 7
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 8
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 9
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 10
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 11
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 12
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 13
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 14
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 15
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 16
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 17
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 18
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 19
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 20
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 21
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 22
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 23
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 24
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 25
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 26
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 27
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 28
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 29
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 30
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 31
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 32
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 33
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 34
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 35
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 36
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 37
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 38
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 39
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 40
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 41
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 42
            },
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 43
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 44
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 45
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 46
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 47
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 48
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 49
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 50
            }
             ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 51
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 52
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 53
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 54
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 55
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 56
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 57
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 58
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 59
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 60
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 61
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 62
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 63
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 64
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 65
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 66
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 67
            }

                        ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 69
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 70
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 71
            }
                        ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 72
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 73
            }
                        ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 74
            }
                        ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 75
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 76
            }
                        ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 77
            }
                                    ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 78
            }
                                                ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 79
            }
            ,
            new RoleMenuPermission()
            {
                RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                NavigationMenuId = 80
            },
                        new RoleMenuPermission()
                        {
                            RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                            NavigationMenuId = 81
                        },
                                    new RoleMenuPermission()
                                    {
                                        RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                                        NavigationMenuId = 82
                                    }
                                    ,
                                    new RoleMenuPermission()
                                    {
                                        RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                                        NavigationMenuId = 83
                                    },
                                    new RoleMenuPermission()
                                    {
                                        RoleId = DefaultValues.SUPER_USER_ROLE_ID,
                                        NavigationMenuId = 84
                                    }
            );
            #endregion

            #endregion
        }
        public static string GetHashedPassword(User user, string password)
        {
            PasswordHasher<User> pass = new PasswordHasher<User>();
            string hashed = pass.HashPassword(user, password);
            return hashed;
        }
    }
}
