using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StorePanel.Core.Models;
using StorePanel.Core.Utility;
using StorePanel.Infrastructure.Helpers;
using StorePanel.Infrastructure.Repository;
using StorePanel.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StorePanel.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class DashboardController : Controller
    {
        private IUserRepository _usersRepo;
        private ICustomersRepository _customerRepo;
        private IInvoicesRepository _invoiceRepo;
        private IGeoDivisionsRepository _geoDivisonsRepo;

        private UserManager<User> _userManager;

        public DashboardController(IUserRepository userRepository
        ,ICustomersRepository customersRepository
        ,IInvoicesRepository invoicesRepository
        ,IGeoDivisionsRepository geoDivisionsRepository
        ,UserManager<User> userManager)
        {
            _usersRepo = userRepository;

            _customerRepo = customersRepository;

            _geoDivisonsRepo = geoDivisionsRepository;

            _invoiceRepo = invoicesRepository;

            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var customer = _customerRepo.GetUserCustomer(user.Id);

            ViewData["GeoDivisions"] = _geoDivisonsRepo.GetDefaultQuery().Select(a =>
            new SelectListItem()
            {
                Text = a.Title,
                Value = a.Id.ToString(),
                Selected = (a.Id == customer.GeoDivisionId)
            }).ToList();

            if (customer!=null)
            {
                var invoices = _invoiceRepo.GetCustomerInvoices(customer.Id);
                var invoicesVm = new List<CustomerInvoiceViewModel>();
                foreach (var item in invoices)
                    invoicesVm.Add(new CustomerInvoiceViewModel(item));

                var vm = new CustomerDashboardViewModel()
                {
                    Customer = customer,
                    Invoices = invoicesVm.OrderByDescending(x=>x.Id).ToList()
                };

                return View(vm);
            }

        

            var vm2 = new CustomerDashboardViewModel()
            {
                Customer = customer,
                Invoices = null
            };

            return View(vm2);

        }
        [HttpPost]
        public async Task<IActionResult> Index(CustomerDashboardViewModel form)
        {
            if (ModelState.IsValid)
            {
                #region Check for duplicate username or email

                if (form.Customer.User != null)

                {
                    if (await _usersRepo.UserNameExists(form.Customer.User.UserName, form.Customer.UserId))
                    {
                        ViewBag.Message = "کاربر دیگری با همین نام کاربری در سیستم ثبت شده";
                        ViewBag.GeoDivisionId = new SelectList(_geoDivisonsRepo.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", form.Customer.GeoDivisionId);

                        return View(form);
                    }
                }
                #endregion



                var user = await _usersRepo.GetById(form.Customer.UserId);
                user.FirstName = form.Customer.User.FirstName;
                user.LastName = form.Customer.User.LastName;
                user.Email = form.Customer.User.Email;
                user.UserName = form.Customer.User.UserName;


                await _usersRepo.UpdateUser(user);

                var customer = await _customerRepo.GetById(form.Customer.Id);

                customer.NationalCode = form.Customer.NationalCode;
                customer.Address = form.Customer.Address;
                customer.PostalCode = form.Customer.PostalCode;
                customer.GeoDivisionId = form.Customer.GeoDivisionId;
                await _customerRepo.Update(customer);

                return RedirectToAction("Index");
            }
            ViewBag.GeoDivisionId = new SelectList(_geoDivisonsRepo.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", form.Customer.GeoDivisionId);
            return View(form);
        }
        public async Task<IActionResult> EditMyProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            var customer = _customerRepo.GetUserCustomer(user.Id);
            if (customer == null)
            {
                return NotFound();
            }

            var form = new EditCustomerViewModel()
            {
                UserId = customer.User.Id,
                UserName = customer.User.UserName,
                CustomerId = customer.Id,
                FirstName = customer.User.FirstName,
                LastName = customer.User.LastName,
                Email = customer.User.Email,
                PhoneNumber = customer.User.PhoneNumber,
                Avatar = customer.User.Avatar,
                NationalCode = customer.NationalCode,
                Address = customer.Address,
                PostalCode = customer.PostalCode,
                GeoDivisionId = customer.GeoDivisionId
            };
            ViewBag.GeoDivisionId = _geoDivisonsRepo.GetGeoDivisionsByType((int)GeoDivisionType.State);
            return View(form);
        }
        [HttpPost]
        public async Task<IActionResult> EditMyProfile(EditCustomerViewModel form, IFormFile UserAvatar)
        {
            if (ModelState.IsValid)
            {
                #region Check for duplicate username or email

                if (form.UserName != null)

                {
                    if (await _usersRepo.UserNameExists(form.UserName, form.UserId))
                    {
                        ViewBag.Message = "کاربر دیگری با همین نام کاربری در سیستم ثبت شده";
                        ViewBag.GeoDivisionId = new SelectList(_geoDivisonsRepo.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", form.GeoDivisionId);

                        return View(form);
                    }
                }
                if (await _usersRepo.PhoneNumberExists(form.PhoneNumber, form.UserId))
                {
                    ViewBag.Message = "کاربر دیگری با همین شماره تلفن در سیستم ثبت شده";
                    ViewBag.GeoDivisionId = new SelectList(_geoDivisonsRepo.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", form.GeoDivisionId);
                    return View(form);
                }
                if (await _usersRepo.EmailExists(form.Email, form.UserId))
                {
                    ViewBag.Message = "کاربر دیگری با همین ایمیل در سیستم ثبت شده";
                    ViewBag.GeoDivisionId = new SelectList(_geoDivisonsRepo.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", form.GeoDivisionId);
                    return View(form);
                }
                #endregion

                #region Upload Image
                if (UserAvatar != null)
                {
                    var imageName = await ImageHelper.SaveImage(UserAvatar, 670, 400, true);
                    form.Avatar = imageName;
                }
                #endregion


                var user =await _usersRepo.GetById(form.UserId);
                user.UserName = form.UserName ?? form.PhoneNumber;
                user.FirstName = form.FirstName;
                user.LastName = form.LastName;
                user.Email = form.Email;
                user.PhoneNumber = form.PhoneNumber;
                user.Avatar = form.Avatar;

                await _usersRepo.UpdateUser(user);

                var customer =await _customerRepo.GetById(form.CustomerId.Value);

                customer.NationalCode = form.NationalCode;
                customer.Address = form.Address;
                customer.PostalCode = form.PostalCode;
                customer.GeoDivisionId = form.GeoDivisionId;
                await _customerRepo.Update(customer);

                return RedirectToAction("Index");
            }
            ViewBag.GeoDivisionId = new SelectList(_geoDivisonsRepo.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", form.GeoDivisionId);
            return View(form);

        }
        public IActionResult ResetMyPassword(string id)
        {
            ViewBag.Message = null;
            ViewBag.UserId = id;
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetMyPassword(ResetMyPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user =await _userManager.GetUserAsync(User);
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Invoices()
        {
            var user = await _userManager.GetUserAsync(User);

            var customer = _customerRepo.GetUserCustomer(user.Id);
            var invoices = _invoiceRepo.GetCustomerInvoices(customer.Id);
            var invoicesVm = new List<CustomerInvoiceViewModel>();
            foreach (var item in invoices)
                invoicesVm.Add(new CustomerInvoiceViewModel(item));

            return View(invoicesVm);
        }

        public IActionResult Detail(int Id)
        {
            var vm = new ViewInvoiceViewModel();
            var invoice = _invoiceRepo.GetInvoice(Id);
            vm.Invoice = invoice;
            vm.PersianDate = new PersianDateTime(invoice.AddedDate).ToString();
            vm.InvoiceItems = new List<InvoiceItemWithMainFeatureViewModel>();
            // Getting Invoice Item SubFeatures
            foreach (var invoiceItem in invoice.InvoiceItems)
            {
                var invoiceItemWithMainFeature = new InvoiceItemWithMainFeatureViewModel
                {
                    InvoiceItem = invoiceItem,
                    MainFeature = _invoiceRepo.GetInvoiceItemsMainFeature(invoiceItem.Id)
                };
                vm.InvoiceItems.Add(invoiceItemWithMainFeature);

            }
            return View(vm);
        }
    }
}
