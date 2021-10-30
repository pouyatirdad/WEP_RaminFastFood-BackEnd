using Microsoft.AspNetCore.Mvc;
using MvcSiteMapProvider;
using SimpleMvcSitemap;
using SimpleMvcSitemap.Routing;
using StorePanel.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Controllers
{
    public class BaseUrlProvider : IBaseUrlProvider
    {
        public Uri BaseUrl => new Uri("https://webpersian.co");
    }

    //public class SitemapController : Controller
    //{
    //    private readonly StorePanelDbContext _context;
    //    public SitemapController(StorePanelDbContext context)
    //    {
    //        _context = context;
    //    }

    //    [Route("sitemap.xml")]
    //    public async Task<IActionResult> Index()
    //    {
    //        //var baseUrl = "https://webpersian.co";

    //        //List<SitemapNode> nodes = new List<SitemapNode>();
    //        //var staticContent = _context.StaticContent.Where(s => s.IsDeleted == false).FirstOrDefault() ?? new StaticContent();
    //        //staticContent.UpdateDate = staticContent.UpdateDate ?? staticContent.InsertDate;
    //        //nodes.Add(new SitemapNode(Url.Action("Index", "Home")) { Priority = 1, LastModificationDate = staticContent.UpdateDate ?? DateTime.Now });
    //        //nodes.Add(new SitemapNode(Url.Action("ContactUs", "Home")) { Priority = .9m, LastModificationDate = staticContent.UpdateDate ?? DateTime.Now });
    //        //nodes.Add(new SitemapNode(Url.Action("AboutUs", "Home")) { Priority = .9m, LastModificationDate = staticContent.UpdateDate ?? DateTime.Now });

    //        //var latestService = _context.OurServices.Where(s => s.IsDeleted == false).OrderByDescending(w => w.InsertDate).ThenByDescending(w => w.UpdateDate).FirstOrDefault() ?? new OurService();
    //        //latestService.UpdateDate = latestService.UpdateDate ?? latestService.InsertDate;
    //        //nodes.Add(new SitemapNode(Url.Action("OurServices", "Home")) { Priority = .8m, LastModificationDate = latestService.UpdateDate ?? DateTime.Now });

    //        //var services = _context.OurServices.Where(w => w.IsDeleted == false).ToList();
    //        //foreach (var service in services)
    //        //{
    //        //    service.UpdateDate = service.UpdateDate ?? service.InsertDate;
    //        //    nodes.Add(new SitemapNode(baseUrl + $"/our-services/{service.Id}/{service.Title.Slugify()}") { Priority = .8m, LastModificationDate = service.UpdateDate ?? DateTime.Now });
    //        //}

    //        //var latestBlog = _context.Articles.Where(s => s.IsDeleted == false).OrderByDescending(w => w.InsertDate).ThenByDescending(w => w.UpdateDate).FirstOrDefault() ?? new Article();
    //        //latestBlog.UpdateDate = latestBlog.UpdateDate ?? latestBlog.InsertDate;
    //        //nodes.Add(new SitemapNode(Url.Action("Index", "Blog")) { Priority = .8m, LastModificationDate = latestBlog.UpdateDate ?? DateTime.Now });

    //        //var blogs = _context.Articles.Where(w => w.IsDeleted == false).ToList();
    //        //foreach (var blog in blogs)
    //        //{
    //        //    blog.UpdateDate = blog.UpdateDate ?? blog.InsertDate;
    //        //    nodes.Add(new SitemapNode(baseUrl + $"/blog/post/{blog.Id}/{blog.Title.Slugify()}") { Priority = .8m, LastModificationDate = blog.UpdateDate ?? DateTime.Now });
    //        //}

    //        //var LastworkSample = _context.WorkSamples.Where(s => s.IsDeleted == false).OrderByDescending(w => w.InsertDate).ThenByDescending(w => w.UpdateDate).FirstOrDefault() ?? new WorkSample();
    //        //LastworkSample.UpdateDate = LastworkSample.UpdateDate ?? LastworkSample.InsertDate;
    //        //nodes.Add(new SiteMapNode(Url.Action("WorkSamples", "Home")) { LastModificationDate = LastworkSample.UpdateDate ?? DateTime.Now });

    //        //var workSamples = _context.WorkSamples.Where(w => w.IsDeleted == false).ToList();
    //        //foreach (var sample in workSamples)
    //        //{
    //        //    sample.UpdateDate = sample.UpdateDate ?? sample.InsertDate;
    //        //    nodes.Add(new SitemapNode(baseUrl + $"/portfolio/{sample.Id}/{sample.Title.Slugify()}") { LastModificationDate = sample.UpdateDate ?? DateTime.Now });
    //        //}


    //        //// get available controllers
    //        //var controllers = Assembly.GetExecutingAssembly().GetTypes()
    //        //    .Where(type => typeof(Controller).IsAssignableFrom(type)
    //        //                   || type.Name.EndsWith("controller")).ToList();

    //        //foreach (var controller in controllers)
    //        //{
    //        //    // get available methods
    //        //    var methods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
    //        //        .Where(method => typeof(IActionResult).IsAssignableFrom(method.ReturnType));

    //        //    foreach (var method in methods)
    //        //    {
    //        //        // add route name in sitemap
    //        //        nodes.Add(new SitemapNode(Url.Action(method.Name, controller.Name)));
    //        //    }
    //        //}

    //        //return new SitemapProvider(new BaseUrlProvider()).CreateSitemap(new SitemapModel(nodes));
    //    }
    }
