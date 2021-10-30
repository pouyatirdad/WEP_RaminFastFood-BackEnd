using DataTablesParser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Repository;
using StorePanel.Web.Areas.Dashboard.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace StorePanel.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize("Permission")]
    public class ProductGroupsController : Controller
    {
        private readonly IProductGroupsRepository _productGroupsRepository;

        private readonly IBrandsRepository _brandsRepository;

        private readonly IFeaturesRepository _featuresRepository;

        public ProductGroupsController(IProductGroupsRepository productGroupsRepository
        ,IBrandsRepository brandsRepository
        ,IFeaturesRepository featuresRepository)
        {
            _productGroupsRepository = productGroupsRepository;

            _brandsRepository = brandsRepository;

            _featuresRepository = featuresRepository;
        }

        public IActionResult Index(bool root = false)
        {
            ViewBag.Root = root;
            return View();
        }

        [AllowAnonymous]
        public IActionResult SubProductGroup(int id, bool root = false)
        {
            ViewBag.Root = root;
            ViewBag.ProductGroupId = id;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public string LoadGrid()
        {

                var parser = new Parser<ProductGroup>(Request.Form, _productGroupsRepository.GetDefaultQuery().AsQueryable());

            return JsonConvert.SerializeObject(parser.Parse(), Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

        }

        [HttpPost]
        [AllowAnonymous]
        public string LoadGridSub(int parentId)
        {


            var parser = new Parser<ProductGroup>(Request.Form, _productGroupsRepository.GetDefaultQuery().Where(x => x.ParentId == parentId).AsQueryable());
            return JsonConvert.SerializeObject(parser.Parse(), Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });


        }


        public IActionResult Create()
        {

            ViewBag.Features = _productGroupsRepository.GetFeatures();
            ViewBag.Brands = _productGroupsRepository.GetBrands();
            ViewBag.ProductGroups = _productGroupsRepository.GetProductGroups();
            return View();
        }

        [HttpPost]
        public int? Create(NewProductGroupViewModel productGroup)
        {
            if (ModelState.IsValid)
            {
                var product = _productGroupsRepository.AddNewProductGroup(productGroup.ParentGroupId, productGroup.Title, productGroup.BrandIds, productGroup.ProductGroupFeatureIds,productGroup.Image);
                return product.Id;
            }

            return null;
        }

        public async Task<IActionResult> Edit(int Id)
        {

            ViewBag.Features =await _featuresRepository.GetAll();
            ViewBag.ProductGroups = _productGroupsRepository.GetProductGroups();

            var model = _productGroupsRepository.GetProductGroup(Id);
            ViewBag.Brands =await _brandsRepository.GetAll();


            return View(model);
        }

        [HttpPost]
        public int? Edit(NewProductGroupViewModel productGroup)
        {
            if (ModelState.IsValid)
            {
                var product = _productGroupsRepository.AddNewProductGroup(productGroup.ParentGroupId, productGroup.Title, productGroup.BrandIds, productGroup.ProductGroupFeatureIds, productGroup.Image);
                return product.Id;
            }

            return null;
        }

        public async Task<IActionResult> Delete(int id)
        {
            return PartialView(await _productGroupsRepository.GetById(id));
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productGroupsRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
