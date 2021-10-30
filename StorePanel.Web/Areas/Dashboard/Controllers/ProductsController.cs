using DataTablesParser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Repository;
using StorePanel.Web.Areas.Dashboard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace StorePanel.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize("Permission")]
    public class ProductsController : Controller
    {
        private readonly IProductsRepository _productRepository;

        private readonly IProductGroupsRepository _productGroupsRepository;

        private readonly IProductMainFeaturesRepository _productMainFeaturesRepository;

        private readonly IProductFeatureValuesRepository _featureRepo;

        public ProductsController(IProductsRepository productsRepository
        ,IProductGroupsRepository productGroupsRepository
        ,IProductMainFeaturesRepository productMainFeaturesRepository
        , IProductFeatureValuesRepository productFeatureValues)
        {
            _productRepository = productsRepository;

            _productGroupsRepository = productGroupsRepository;

            _productMainFeaturesRepository = productMainFeaturesRepository;

            _featureRepo = productFeatureValues;
        }

        public IActionResult Index(bool root = false)
        {
            ViewBag.Root = root;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public string LoadGrid()
        {
                var parser = new Parser<Product>(Request.Form,_productRepository.GetDefaultQuery().AsQueryable());
                return JsonConvert.SerializeObject(parser.Parse());
        }

        public IActionResult Create()
        {
            ViewBag.ProductGroups = _productGroupsRepository.GetProductGroups();
            return View();
        }

        [HttpPost]
        public async Task<int?> Create(NewProductViewModel product)
        {
            if (!ModelState.IsValid) return null;
            var prod = new Product();
            prod.Title = product.Title;
            prod.ShortDescription = product.ShortDescription;
            prod.Description = HttpUtility.UrlDecode(product.Description, System.Text.Encoding.Default);
            prod.BrandId = product.Brand;
            prod.ProductGroupId = product.ProductGroup;
            prod.Rate = product.Rate * 10;
            prod.ShortDescription = product.ShortDescription;
            var addProduct =await _productRepository.Add(prod);
            #region Adding Product Features

            foreach (var feature in product.ProductFeatures)
            {
                if (feature.IsMain)
                {
                    var model = new ProductMainFeature();
                    model.ProductId = addProduct.Id;
                    model.FeatureId = feature.FeatureId;
                    model.SubFeatureId = feature.SubFeatureId;
                    model.Value = feature.Value;
                    model.Quantity = feature.Quantity ?? 0;
                    model.Price = feature.Price ?? 0;
                    _productRepository.AddProductMainFeature(model);
                }
                else
                {
                    var model = new ProductFeatureValue();
                    model.ProductId = addProduct.Id;
                    model.FeatureId = feature.FeatureId;
                    model.SubFeatureId = feature.SubFeatureId;
                    model.Value = feature.Value;
                    _productRepository.AddProductFeature(model);
                }
            }
            #endregion
            return addProduct.Id;
        }

        [AllowAnonymous]
        public JsonResult GetProductGroupFeatures(int id)
        {
            var features = _productGroupsRepository.GetProductGroupFeatures(id);
            var obj = features.Select(item => new FeaturesObjViewModel() { Id = item.Id, Title = item.Title }).ToList();
            return Json(obj);
        }

        [AllowAnonymous]
        public JsonResult GetProductFeatures(int id)
        {
            var mainFeatures = _productMainFeaturesRepository.GetProductMainFeatures(id);
            var features = _featureRepo.GetProductFeatures(id);
            var obj = mainFeatures.Select(mainFeature => new ProductFeaturesViewModel()
            {
                ProductId = mainFeature.ProductId,
                FeatureId = mainFeature.FeatureId,
                SubFeatureId = mainFeature.SubFeatureId,
                IsMain = true,
                Value = mainFeature.Value,
                Quantity = mainFeature.Quantity,
                Price = mainFeature.Price
            })
                .ToList();
            obj.AddRange(features.Select(feature => new ProductFeaturesViewModel()
            {
                ProductId = feature.ProductId,
                FeatureId = feature.FeatureId.Value,
                Value = feature.Value,
                IsMain = false,
                SubFeatureId = feature.SubFeatureId
            }));

            return Json(obj.GroupBy(a => a.FeatureId));
        }

        [AllowAnonymous]
        public JsonResult GetProductGroupBrands(int id)
        {
            var brands = _productGroupsRepository.GetProductGroupBrands(id);
            var obj = brands.Select(item => new BrandsObjViewModel() { Id = item.Id, Name = item.Name }).ToList();
            return Json(obj);
        }

        [AllowAnonymous]
        public JsonResult GetFeatureSubFeatures(int id)
        {
            var subFeatures = _productRepository.GetSubFeaturesByFeatureId(id);
            var obj = subFeatures.Select(item => new SubFeaturesObjViewModel() { Id = item.Id, Value = item.Value }).ToList();
            return Json(obj);
        }


        public async Task<IActionResult> Delete(int id)
        {
            return PartialView(await _productRepository.GetById(id));
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(int id)
        {
            ViewBag.ProductGroups = _productGroupsRepository.GetProductGroups();
            var product = _productRepository.GetProduct(id);
            return View(product);
        }
        [HttpPost]
        public async Task<int?> Edit(NewProductViewModel product)
        {
            if (!ModelState.IsValid) return null;
            var prod =await _productRepository.GetById(product.ProductId.Value);
            prod.Title = product.Title;
            prod.ShortDescription = product.ShortDescription;
            prod.Description = HttpUtility.UrlDecode(product.Description, System.Text.Encoding.Default);
            prod.BrandId = product.Brand;
            prod.ProductGroupId = product.ProductGroup;
            prod.Rate = product.Rate;

            var updateProduct = _productRepository.Update(prod);
            #region Removing Previous Product Features
            var productMainFeatures = _productRepository.GetProductMainFeatures(updateProduct.Id);
            foreach (var mainFeature in productMainFeatures)
              await  _productMainFeaturesRepository.Delete(mainFeature.Id);

            var productFeatures = _productRepository.GetProductFeatures(updateProduct.Id);
            foreach (var feature in productFeatures)
                await _featureRepo.Delete(feature.Id);
            #endregion

            #region Adding Product Features

            foreach (var feature in product.ProductFeatures)
            {
                if (feature.IsMain)
                {
                    var model = new ProductMainFeature();
                    model.ProductId = updateProduct.Id;
                    model.FeatureId = feature.FeatureId;
                    model.SubFeatureId = feature.SubFeatureId;
                    model.Value = feature.Value;
                    model.Quantity = feature.Quantity ?? 0;
                    model.Price = feature.Price ?? 0;
                    _productRepository.AddProductMainFeature(model);
                }
                else
                {
                    var model = new ProductFeatureValue();
                    model.ProductId = updateProduct.Id;
                    model.FeatureId = feature.FeatureId;
                    model.SubFeatureId = feature.SubFeatureId;
                    model.Value = feature.Value;
                    _productRepository.AddProductFeature(model);
                }
            }
            #endregion
            return updateProduct.Id;

        }

    }
}
