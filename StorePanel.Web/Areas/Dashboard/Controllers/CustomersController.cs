using DataTablesParser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StorePanel.Core.Models;
using StorePanel.Core.Utility;
using StorePanel.Infrastructure.Context;
using StorePanel.Infrastructure.Helpers;
using StorePanel.Infrastructure.Repository;
using StorePanel.Web.Areas.Dashboard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize("Permission")]
    public class CustomersController : Controller
    {
        private readonly ICustomersRepository _repo;
        private readonly IUserRepository _usersRepo;
        private readonly IGeoDivisionsRepository _geoDivisonsRepo;
        private readonly StorePanelDbContext _context;
        private UserManager<User> _userManager;

        public CustomersController(ICustomersRepository repo, IUserRepository usersRepo, IGeoDivisionsRepository geoDivisonsRepo,StorePanelDbContext context
        ,UserManager<User> userManager)
        {
            _repo = repo;
            _usersRepo = usersRepo;
            _geoDivisonsRepo = geoDivisonsRepo;
            _context = context;
            _userManager = userManager;
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
              var usersGrid = _context.Customers.Include(x=>x.User).Where(x=>x.IsDeleted==false).Select(item =>
                new CustomerGridViewModel()
                {
                    Id = item.Id,
                    LastName=item.User.LastName,
                    Name =item.User.FirstName,
                    Phonenumber=item.User.PhoneNumber
                }
            ).AsQueryable();
            var parser = new Parser<CustomerGridViewModel>(Request.Form, usersGrid);
            return JsonConvert.SerializeObject(parser.Parse());
        }


        public IActionResult Create()
        {
            ViewBag.Message = null;
            ViewBag.GeoDivisionId = new SelectList(_geoDivisonsRepo.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddCustomerViewModel form, IFormFile UserAvatar)
        {
            if (ModelState.IsValid)
            {
                #region Check for duplicate username or email

                if (form.UserName != null)
                {
                    if (await _usersRepo.UserNameExists(form.UserName))
                    {
                        ViewBag.Message = "کاربر دیگری با همین نام کاربری در سیستم ثبت شده";
                        ViewBag.GeoDivisionId = new SelectList(_geoDivisonsRepo.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", form.GeoDivisionId);

                        return View(form);
                    }
                }
                if (await _usersRepo.PhoneNumberExists(form.PhoneNumber))
                {
                    ViewBag.Message = "کاربر دیگری با همین شماره تلفن در سیستم ثبت شده";
                    ViewBag.GeoDivisionId = new SelectList(_geoDivisonsRepo.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", form.GeoDivisionId);

                    return View(form);
                }
                if (await _usersRepo.EmailExists(form.Email))
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

                var userModel = new User()
                {
                    UserName = form.UserName,
                    FirstName = form.FirstName,
                    LastName = form.LastName,
                    Email = form.Email,
                    PhoneNumber = form.PhoneNumber,
                    Avatar = form.Avatar
                };
                userModel.UserName = form.UserName ?? form.PhoneNumber;
                await _usersRepo.CreateUser(userModel, form.Password);
                await _userManager.AddToRoleAsync(userModel, "Customer");

                var customer = new  StorePanel.Core.Models.Customer()
                {
                    UserId = userModel.Id,
                    IsDeleted = false,
                    NationalCode = form.NationalCode,
                    Address = form.Address,
                    PostalCode = form.PostalCode,
                    GeoDivisionId = form.GeoDivisionId
                };
                await _repo.Add(customer);

                return RedirectToAction("Index");
            }

            ViewBag.GeoDivisionId = new SelectList(_geoDivisonsRepo.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", form.GeoDivisionId);
            return View(form);
        }


        public IActionResult Delete(int id)
        {
            return PartialView(_context.Customers.Include(x=>x.User).Where(x=>x.Id==id).FirstOrDefault());
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.Delete(id);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            var customer = _repo.GetCustomer(id);
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
            ViewBag.GeoDivisionId = new SelectList(_geoDivisonsRepo.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", form.GeoDivisionId);
            return View(form);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCustomerViewModel form, IFormFile UserAvatar)
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

                var customer =await _repo.GetById(form.CustomerId.Value);

                customer.NationalCode = form.NationalCode;
                customer.Address = form.Address;
                customer.PostalCode = form.PostalCode;
                customer.GeoDivisionId = form.GeoDivisionId;
                await _repo.Update(customer);

                return RedirectToAction("Index");
            }
            ViewBag.GeoDivisionId = new SelectList(_geoDivisonsRepo.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", form.GeoDivisionId);
            return View(form);

        }

    }
}
