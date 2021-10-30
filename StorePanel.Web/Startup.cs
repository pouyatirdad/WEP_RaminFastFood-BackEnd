using AspNetCore.ReCaptcha;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Serilog;
using StorePanel.Core.Models;
using StorePanel.Core.Utility;
using StorePanel.Infrastructure;
using StorePanel.Infrastructure.Context;
using StorePanel.Infrastructure.Helpers;
using StorePanel.Infrastructure.Service;
using StorePanel.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using WebMarkupMin.AspNetCore3;

namespace StorePanel.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // Email Settings
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            #region Identity Configuration
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<StorePanelDbContext>()
            .AddDefaultTokenProviders()
            .AddErrorDescriber<LocalizedIdentityErrorDescriber>();

            services.ConfigureApplicationCookie(options =>
            {
                
                options.AccessDeniedPath = new PathString("/Auth/AccessDenied");
                options.LoginPath = new PathString("/Auth/Login");
            });

            #endregion

            services.AddDbContext<StorePanelDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddHttpContextAccessor();
            services.AddInfrastructure();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("fa");

            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddScoped<IUserPermissionHelper, UserPermissionHelper>();
            services.AddSingleton<HtmlEncoder>(
            HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin,
            UnicodeRanges.Arabic }));

            services.AddProgressiveWebApp();

            //services.AddWebMarkupMin(options =>
            //{
            //    options.AllowMinificationInDevelopmentEnvironment = true;
            //    options.AllowCompressionInDevelopmentEnvironment = true;
            //})
            //.AddHtmlMinification()
            //.AddHttpCompression()
            //.AddXmlMinification();

            services.AddReCaptcha(Configuration.GetSection("ReCaptcha"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            MyAppContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());

            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles")),
                RequestPath = "/UploadedFiles",

            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                RequestPath = string.Empty,

            });
            app.UseSerilogRequestLogging();
            app.UseRouting();
            //app.UseWebMarkupMin();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default-area",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
