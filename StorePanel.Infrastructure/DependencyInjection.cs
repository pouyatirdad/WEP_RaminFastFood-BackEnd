using Microsoft.Extensions.DependencyInjection;
using StorePanel.Infrastructure.Repository;
using StorePanel.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorePanel.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            #region Add Repositories
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ISystemParameterRepository, SystemParameterRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IArticleCommentRepository, ArticleCommentRepository>();
            services.AddScoped<IContactUsFormRepository, ContactUsFormRepository>();
            services.AddScoped<IFaqCategoryRepository, FaqCategoryRepository>();
            services.AddScoped<IStaticContentRepository, StaticContentRepository>();
            services.AddScoped<IFaqRepository, FaqRepository>();
            services.AddScoped<IBrandsRepository, BrandsRepository>();
            services.AddScoped<IFeaturesRepository, FeaturesRepository>();
            services.AddScoped<ISubFeaturesRepository, SubFeaturesRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IProductGroupsRepository, ProductGroupsRepository>();
            services.AddScoped<IProductCommentsRepository, ProductCommentsRepository>();
            services.AddScoped<IOffersRepository, OffersRepository>();
            services.AddScoped<IDiscountsRepository, DiscountsRepository>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<ICustomersRepository, CustomersRepository>();
            services.AddScoped<IGeoDivisionsRepository, GeoDivisionsRepository>();
            services.AddScoped<IInvoicesRepository, InvoicesRepository>();
            services.AddScoped<IProductFeatureValuesRepository, ProductFeatureValuesRepository>();
            services.AddScoped<IProductMainFeaturesRepository, ProductMainFeaturesRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasicdefinitionsRepository, BasicdefinitionsRepository>();
            services.AddScoped<ISliderRepository, SliderRepository>();
            #endregion

            return services;
        }
    }
}
