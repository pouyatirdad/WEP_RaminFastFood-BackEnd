using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using StorePanel.Core.Models;

using Microsoft.AspNetCore.Mvc.Rendering;
using StorePanel.Core.Utility;

using StorePanel.Infrastructure.Dtos.Product;
using StorePanel.Infrastructure.Repository;
using StorePanel.Infrastructure.Service;
using StorePanel.Web.Models;
using StorePanel.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZarinPal.Class;
using Dto.Payment;

namespace StorePanel.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductsRepository _productsRepository;

        private readonly IProductMainFeaturesRepository _productMainFeaturesRepository;

        private readonly IProductService _productService;

        private readonly IProductCommentsRepository _productCommentsRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ICustomersRepository _customersRepository;

        private UserManager<User> _userManager;

        private readonly IGeoDivisionsRepository _geoDivisionsRepository;

        private readonly IInvoicesRepository _invoicesRepository;

        private readonly IBasicdefinitionsRepository _basicdefinitionsRepository;


        public ShopController(IProductsRepository productsRepository
            , IProductCommentsRepository productCommentsRepository
        , IProductMainFeaturesRepository productMainFeaturesRepository
        , IProductService productService
        , IHttpContextAccessor httpContextAccessor
        , ICustomersRepository customersRepository
        , UserManager<User> userManager
        , IGeoDivisionsRepository geoDivisionsRepository
        , IInvoicesRepository invoicesRepository
        ,IBasicdefinitionsRepository basicdefinitionsRepository)
        {
            _productsRepository = productsRepository;

            _productMainFeaturesRepository = productMainFeaturesRepository;

            _productService = productService;

            _httpContextAccessor = httpContextAccessor;

            _productCommentsRepository = productCommentsRepository;

            _customersRepository = customersRepository;

            _userManager = userManager;

            _geoDivisionsRepository = geoDivisionsRepository;

            _invoicesRepository = invoicesRepository;

            _basicdefinitionsRepository = basicdefinitionsRepository;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 9, int? ProductGroupId = null)
        {

            int paresh = (page - 1) * pageSize;
            ViewBag.PageID = page;
            ViewBag.PageSize = pageSize;
            var products = new List<ProductWithPriceDto> { };
            if (ProductGroupId == null)
            {
                products = _productService.GetLatestProductsWithPrice().Result;
            }
            else
            {
                products = _productService.GetLatestProductsWithPrice().Result.Where(x => x.ProductGroupId == ProductGroupId).ToList();
            }

            var vm = new List<ProductWithPriceViewModel>();
            foreach (var product in products)
            {
                vm.Add(new ProductWithPriceViewModel(product));
            }


            int totalCount = vm.Count();
            ViewBag.All = totalCount;

            return View(vm.Skip(paresh).Take(pageSize).ToList());
        }

        public ActionResult Name(int page = 1, int pageSize = 9)
        {
            int paresh = (page - 1) * pageSize;
            ViewBag.PageID = page;
            ViewBag.PageSize = pageSize;
            var products = _productService.GetLatestProductsWithPrice().Result.OrderByDescending(x => x.ShortTitle).ToList();
            var vm = new List<ProductWithPriceViewModel>();
            foreach (var product in products)
            {
                vm.Add(new ProductWithPriceViewModel(product));
            }


            int totalCount = vm.Count();
            ViewBag.All = totalCount;

            return View("index", vm.Skip(paresh).Take(pageSize).ToList());
        }
        public ActionResult Price(int page = 1, int pageSize = 9)
        {
            int paresh = (page - 1) * pageSize;
            ViewBag.PageID = page;
            ViewBag.PageSize = pageSize;
            var products = _productService.GetLatestProductsWithPrice().Result.OrderByDescending(x => x.Price).ToList();
            var vm = new List<ProductWithPriceViewModel>();
            foreach (var product in products)
            {
                vm.Add(new ProductWithPriceViewModel(product));
            }


            int totalCount = vm.Count();
            ViewBag.All = totalCount;

            return View("index", vm.Skip(paresh).Take(pageSize).ToList());
        }

        public IActionResult Detail(int Id)
        {
            var data = _productsRepository.GetProduct(Id);

            var cms = _productCommentsRepository.GetProductComments(Id);

            data.ProductComments = cms;

            ViewBag.MainFeatureId = _productMainFeaturesRepository.GetProductMainFeaturesID(Id).Id;

            return View(data);
        }

        [ValidateAntiForgeryToken]
        public IActionResult SearchResult(string txtstring)
        {
            ViewBag.searchString = txtstring;
            var products = _productService.SearchProductWithPrice(txtstring).Result;
            var vm = new List<ProductWithPriceViewModel>();
            foreach (var product in products)
            {
                vm.Add(new ProductWithPriceViewModel(product));
            }

            return View(vm);
        }
        [HttpPost]
        public async Task AddToCart(int productId, int? mainFeatureId)
        {
            try
            {
                var cartModel = new CartModel();
                var cartItemsModel = new List<CartItemModel>();



                #region Checking for cookie

                if (!string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Cookies["cart"]))
                {
                    string cartJsonStr = _httpContextAccessor.HttpContext.Request.Cookies["cart"]; ;
                    cartModel = new CartModel(cartJsonStr);
                    cartItemsModel = cartModel.CartItems;
                }
                #endregion

                ProductWithPriceDto product;
                int productStockCount;
                if (mainFeatureId == null)
                {
                    mainFeatureId = _productMainFeaturesRepository.GetByProductId(productId).Id;
                }
                product = await _productService.CreateProductWithPriceDto(productId, mainFeatureId.Value);
                productStockCount = await _productService.GetProductStockCount(productId, mainFeatureId.Value);

                if (productStockCount > 0)
                {
                    if (cartItemsModel.Any(i => i.Id == productId && i.MainFeatureId == mainFeatureId.Value))
                    {
                        if (cartItemsModel.FirstOrDefault(i => i.Id == productId && i.MainFeatureId == mainFeatureId.Value).Quantity < productStockCount)
                        {
                            cartItemsModel.FirstOrDefault(i => i.Id == productId && i.MainFeatureId == mainFeatureId.Value).Quantity += 1;
                            cartModel.TotalPrice += product.PriceAfterDiscount;
                        }
                    }
                    else
                    {
                        cartItemsModel.Add(new CartItemModel()
                        {
                            Id = product.Id,
                            ProductName = product.ShortTitle,
                            Price = product.PriceAfterDiscount,
                            Quantity = 1,
                            MainFeatureId = mainFeatureId.Value,
                            Image = product.Image
                        });
                        cartModel.TotalPrice += product.PriceAfterDiscount;
                    }
                    cartModel.CartItems = cartItemsModel;
                    var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(cartModel);

                    Set("cart", jsonStr, 10);
                }
            }
            catch (Exception e)
            {


                string cartJsonStr = Request.Cookies["cart"];


                Set("cart", cartJsonStr, 10);
            }
        }

        [HttpPost]
        public void RemoveFromCart(int productId, int? mainFeatureId, string complete = null)
        {
            try
            {
                var cartModel = new CartModel();

                #region Checking for cookie


                if (!string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Cookies["cart"]))
                {
                    string cartJsonStr = _httpContextAccessor.HttpContext.Request.Cookies["cart"];
                    cartModel = new CartModel(cartJsonStr);
                }
                #endregion

                if (cartModel.CartItems.Any(i => i.Id == productId && i.MainFeatureId == mainFeatureId))
                {
                    var itemToRemove = cartModel.CartItems.FirstOrDefault(i => i.Id == productId && i.MainFeatureId == mainFeatureId);
                    if (complete == "true" || itemToRemove.Quantity < 2)
                    {
                        cartModel.TotalPrice -= itemToRemove.Price * itemToRemove.Quantity;
                        cartModel.CartItems.Remove(itemToRemove);
                    }
                    else if (complete == "false")
                    {
                        cartModel.TotalPrice -= itemToRemove.Price;
                        cartModel.CartItems.FirstOrDefault(i => i.Id == productId && i.MainFeatureId == mainFeatureId).Quantity -= 1;
                    }
                }
                var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(cartModel);
                Set("cart", jsonStr, 10);
            }
            catch (Exception e)
            {
                string cartJsonStr = Request.Cookies["cart"];


                Set("cart", cartJsonStr, 10);
            }
        }

        public void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }


        public IActionResult Cart()
        {
            var cartModel = new CartModel();

            try
            {

                if (!string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Cookies["cart"]))
                {
                    string cartJsonStr = _httpContextAccessor.HttpContext.Request.Cookies["cart"];
                    cartModel = new CartModel(cartJsonStr);
                }

            }
            catch (Exception e)
            {
                string cartJsonStr = Request.Cookies["cart"];


                Set("cart", cartJsonStr, 10);
            }
            return View(cartModel);
        }

        public IActionResult CartSection()
        {
            try
            {
                var cartModel = new CartModel();
                cartModel.CartItems = new List<CartItemModel>();

                if (!string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Cookies["cart"]))
                {
                    string cartJsonStr = _httpContextAccessor.HttpContext.Request.Cookies["cart"];
                    cartModel = new CartModel(cartJsonStr);
                }
                return PartialView(cartModel);

            }
            catch (Exception e)
            {



                Set("cart", "", 10);

                var cartModel = new CartModel();
                cartModel.CartItems = new List<CartItemModel>();

                if (!string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Cookies["cart"]))
                {
                    string cartJsonStr = _httpContextAccessor.HttpContext.Request.Cookies["cart"];
                    cartModel = new CartModel(cartJsonStr);
                }
                return PartialView(cartModel);

            }
        }

        public IActionResult WishList()
        {
            var wishListModel = new WishListModel();

            try
            {

                if (!string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Cookies["wishlist"]))
                {
                    string cartJsonStr = _httpContextAccessor.HttpContext.Request.Cookies["wishlist"];
                    wishListModel = new WishListModel(cartJsonStr);
                }

            }
            catch (Exception e)
            {
                string wishlistJsonStr = Request.Cookies["wishlist"];


                Set("wishlist", wishlistJsonStr, 10);
            }

            return View(wishListModel);
        }


        [HttpPost]
        public async Task AddToWishList(int productId)
        {
            try
            {
                var withListModel = new WishListModel();
                var withListItemsModel = new List<WishListItemModel>();

                #region Checking for cookie

                if (!string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Cookies["wishlist"]))
                {
                    string cartJsonStr = _httpContextAccessor.HttpContext.Request.Cookies["wishlist"];
                    withListModel = new WishListModel(cartJsonStr);
                    withListItemsModel = withListModel.WishListItems;
                }
                #endregion

                var product = await _productsRepository.GetById(productId);
                if (withListItemsModel.Any(i => i.Id == productId) == false)
                {
                    withListItemsModel.Add(new WishListItemModel()
                    {
                        Id = product.Id,
                        ProductName = product.ShortDescription,
                        Image = product.Image
                    });
                }
                withListModel.WishListItems = withListItemsModel;
                var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(withListModel);
                Set("wishlist", jsonStr, 10);
            }
            catch (Exception e)
            {
                string wishlistJsonStr = Request.Cookies["wishlist"];


                Set("wishlist", wishlistJsonStr, 10);
            }
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Checkout()
        {
            #region Getting Cart

            #endregion
            var cartModel = new CartModel();

            if (!string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Cookies["cart"]))
            {
                string cartJsonStr = _httpContextAccessor.HttpContext.Request.Cookies["cart"];
                cartModel = new CartModel(cartJsonStr);
            }
            ViewBag.InvoiceNumber = GenerateInvoiceNumber();

            #region Creating CheckoutForm Fields
            var user = await _userManager.GetUserAsync(User);
            var currentCustomer = _customersRepository.GetUserCustomer(user.Id);
            var form = new CheckoutForm();
            form.Address = currentCustomer.Address;
            form.Name = $"{currentCustomer.User.FirstName} {currentCustomer.User.LastName}";
            form.RegisterDate = DateTime.Now;
            form.InvoiceNumber = GenerateInvoiceNumber();
            form.Email = currentCustomer.User.Email;
            form.Phone = currentCustomer.User.PhoneNumber;
            form.PostalCode = currentCustomer.PostalCode;

            ViewBag.PersianRegisterDate = new PersianDateTime(form.RegisterDate).ToString("dddd d MMMM yyyy");
            ViewBag.GeoDivisionId = _geoDivisionsRepository.GetGeoDivisionsByType((int)GeoDivisionType.State);
            #endregion

            var vm = new CheckoutViewModel()
            {
                Cart = cartModel,
                Form = form
            };

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Checkout(CheckoutForm form, int GeoDivisionId)
        {
            var errors = new List<string>();
            if (ModelState.IsValid)
            {
                #region Getting Cart
                var cartModel = new CartModel();

                if (!string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Cookies["cart"]))
                {
                    string cartJsonStr = _httpContextAccessor.HttpContext.Request.Cookies["cart"];
                    cartModel = new CartModel(cartJsonStr);
                }
                #endregion
                var user = await _userManager.GetUserAsync(User);
                var currentCustomer = _customersRepository.GetUserCustomer(user.Id);
                if (currentCustomer != null)
                {
                    try
                    {
                        #region Setting Invoice Fields
                        var invoice = new Invoice();
                        invoice.AddedDate = DateTime.Now;
                        invoice.InvoiceNumber = form.InvoiceNumber;
                        invoice.Address = form.Address;
                        invoice.CustomerId = currentCustomer.Id;
                        invoice.CustomerName = form.Name;
                        invoice.Email = form.Email;
                        invoice.Phone = form.Phone;
                        invoice.Address = form.Address;
                        invoice.PostalCode = form.PostalCode;
                        invoice.GeoDivisionId = GeoDivisionId;
                        invoice.IsDeleted = false;
                        invoice.IsPayed = false;
                        #endregion


                        #region Getting cart items from db

                        long invoiceTotalPrice = 0;
                        var invoiceItems = new List<InvoiceItem>();
                        foreach (var item in cartModel.CartItems)
                        {
                            var product = await _productService.CreateProductWithPriceDto(item.Id, item.MainFeatureId);
                            var productStockCount = await _productService.GetProductStockCount(item.Id, item.MainFeatureId);

                            if (productStockCount >= item.Quantity)
                            {
                                var mainFeature = await _productMainFeaturesRepository.GetById(item.MainFeatureId);
                                if (mainFeature != null && mainFeature.IsDeleted == false)
                                {
                                    invoiceTotalPrice += product.PriceAfterDiscount * item.Quantity;
                                    var invoiceItem = new InvoiceItem();
                                    invoiceItem.InvoiceId = 0; //Setting it to 0 for now going to update it after saving invoice
                                    invoiceItem.ProductId = product.Id;
                                    invoiceItem.Price = product.PriceAfterDiscount;
                                    invoiceItem.Quantity = item.Quantity;
                                    invoiceItem.TotalPrice = product.PriceAfterDiscount * item.Quantity;
                                    invoiceItem.MainFeatureId = item.MainFeatureId;
                                    invoiceItem.IsDeleted = false;
                                    invoiceItems.Add(invoiceItem);
                                }
                                else
                                {
                                    errors.Add($"محصول ( {product.ShortTitle} ) با مشخصات درخواستی شما موجود نیست لطفا سبد خرید خود را بروزرسانی کنید");
                                }
                            }
                            else
                            {
                                errors.Add($"محصول ( {product.ShortTitle} ) به تعداد درخواستی شما موجود نیست لطفا سبد خرید خود را بروزرسانی کنید");
                            }
                        }
                        if (errors.Count == 0)
                        {
                            //Adding Invoice
                            invoice.TotalPrice = invoiceTotalPrice;
                            await _invoicesRepository.Add(invoice);

                            // Adding invoice Items
                            foreach (var inv in invoiceItems)
                            {
                                inv.InvoiceId = invoice.Id;
                                _invoicesRepository.AddInvoiceItem(inv, user);
                            }
                            Set("cart", "", 10);

                            #region Payment request
                            var basicdefinitions = await _basicdefinitionsRepository.GetById(1);
                            if (basicdefinitions.ZarinPalApi !=null && String.IsNullOrEmpty(basicdefinitions.ZarinPalApi))
                            {
                                return Redirect("/Payment/PaymentRequest?Id=" + invoice.Id);
                            }
                            else
                            {

                                return Redirect("/DemoPayment/PaymentRequest?Id=" + invoice.Id);
                            }
                            #endregion

                            //return RedirectToAction("CheckoutSummary", new { id = invoice.Id });
                        }
                        #endregion
                        //ViewBag.Errors = errors;
                        //ViewBag.PersianRegisterDate = new PersianDateTime(form.RegisterDate).ToString("dddd d MMMM yyyy");
                        //ViewBag.GeoDivisionId = new SelectList(_geoDivisionsRepository.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", form.GeoDivisionId);
                        //var vm = new CheckoutViewModel()
                        //{
                        //    Cart = cartModel,
                        //    Form = form
                        //};
                        //return View(vm);
                    }
                    catch (Exception e)
                    {
                        errors.Add("سبد خرید معتبر نیست.");
                        ViewBag.Errors = errors;
                        ViewBag.PersianRegisterDate = new PersianDateTime(form.RegisterDate).ToString("dddd d MMMM yyyy");
                        ViewBag.GeoDivisionId = new SelectList(_geoDivisionsRepository.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", form.GeoDivisionId);
                        var vm = new CheckoutViewModel()
                        {
                            Cart = cartModel,
                            Form = form
                        };
                        return View(vm);
                    }
                }
            }
            return Redirect("/Customer/Auth/Login");
        }

        public string GenerateInvoiceNumber()
        {
            var bytes = Guid.NewGuid().ToByteArray();
            var code = "";
            for (int i = 0; code.Length <= 16 && i < bytes.Length; i++)
            {
                code += (bytes[i] % 10).ToString();
            }

            return code;
        }

        [Authorize(Roles = "Customer")]
        public IActionResult CheckoutSummary(int id)
        {
            var invoice = _invoicesRepository.GetInvoiceWithGeo(id);
            var invoiceItems = _invoicesRepository.GetInvoiceItemsByInvoiceId(id);
            var vm = new InvoiceDetails()
            {
                Invoice = invoice,
                InvoiceItems = invoiceItems,
                PersianRegisterDate = new PersianDateTime(invoice.AddedDate).ToString("dddd d MMMM yyyy")
            };
            return View(vm);
        }
        [HttpPost]
        public string SendComment(ProductComment comment)
        {


            if (ModelState.IsValid)
            {
                comment.AddedDate = DateTime.Now;
                _productCommentsRepository.Add(comment);
                return "success";
            }
            return "fail";
        }
    }
}
