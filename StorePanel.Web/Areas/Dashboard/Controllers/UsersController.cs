using DataTablesParser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly StorePanelDbContext _context;


        public UsersController(IUserRepository userRepo, RoleManager<IdentityRole> roleManager, UserManager<User> userManager, StorePanelDbContext context)
        {
            _userRepo = userRepo;
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
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
            var usersGrid = _context.Users.Select(item =>
                new UserGridViewModel()
                {
                    Id = item.Id,
                    Email = item.Email,
                    Name = $"{item.FirstName} {item.LastName}" ?? "-",
                    Roles = _context.UserRoles.Where(ur => ur.UserId == item.Id).Select(ur => _context.Roles.FirstOrDefault(r => r.Id == ur.RoleId)).Select(r => r.Name).ToList()
                }
            ).AsQueryable();
            var parser = new Parser<UserGridViewModel>(Request.Form, usersGrid);
            return JsonConvert.SerializeObject(parser.Parse());
        }
        public ActionResult Create()
        {
            ViewBag.Message = null;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddUserViewModel form, IFormFile UserAvatar)
        {
            if (ModelState.IsValid)
            {
                #region Check for duplicate username or email
                if (await _userRepo.UserNameExists(form.UserName))
                {
                    ModelState.AddModelError(string.Empty, "کاربر دیگری با همین نام در سیستم ثبت شده");
                    return View(form);
                }
                if (await _userRepo.EmailExists(form.Email))
                {
                    ModelState.AddModelError(string.Empty, "کاربر دیگری با همین ایمیل در سیستم ثبت شده");
                    return View(form);
                }
                #endregion

                #region Upload Image
                if (UserAvatar != null)
                {
                    var imageName = await ImageHelper.SaveImage(UserAvatar, 400, 400, "UserAvatars", true);

                    form.Avatar = imageName;
                }
                #endregion
                var userModel = new User
                {
                    UserName = form.UserName,
                    FirstName = form.FirstName,
                    LastName = form.LastName,
                    PhoneNumber = form.PhoneNumber,
                    Information = form.Information,
                    Email = form.Email,
                    Avatar = form.Avatar
                };

                await _userManager.CreateAsync(userModel, form.Password);
                await _userManager.AddToRoleAsync(userModel, "User");

                return RedirectToAction("Index");
            }

            return View(form);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Information = user.Information,
                Email = user.Email,
                Avatar = user.Avatar
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel user, IFormFile UserAvatar)
        {

            if (ModelState.IsValid)
            {
                #region Check for duplicate username or email
                if (await _userRepo.UserNameExists(user.UserName, user.Id))
                {
                    ModelState.AddModelError(string.Empty, "کاربر دیگری با همین نام در سیستم ثبت شده");
                    return View(user);
                }
                if (await _userRepo.EmailExists(user.Email, user.Id))
                {
                    ModelState.AddModelError(string.Empty, "کاربر دیگری با همین ایمیل در سیستم ثبت شده");
                    return View(user);
                }
                #endregion

                #region Upload Image
                if (UserAvatar != null)
                {
                    var imageName = await ImageHelper.SaveImage(UserAvatar, 400, 400, "UserAvatars", true);

                    user.Avatar = imageName;
                }
                #endregion
                var prevUser = await _userManager.FindByIdAsync(user.Id);
                prevUser.Email = user.Email;
                prevUser.FirstName = user.FirstName;
                prevUser.LastName = user.LastName;
                prevUser.UserName = user.UserName;
                prevUser.PhoneNumber = user.PhoneNumber;
                prevUser.Information = user.Information;

                prevUser.Avatar = user.Avatar;
                var result = await _userRepo.UpdateUser(prevUser);
                return RedirectToAction("Index");
            }
            
            return View(user);

        }
        public async Task<IActionResult> EditMyProfile()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }
            var model = new EditUserViewModel
            {
                Id = currentUser.Id,
                UserName = currentUser.UserName,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                PhoneNumber = currentUser.PhoneNumber,
                Information = currentUser.Information,
                Email = currentUser.Email,
                Avatar = currentUser.Avatar
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditMyProfile(EditUserViewModel user, IFormFile UserAvatar)
        {

            if (ModelState.IsValid)
            {
                #region Check for duplicate username or email
                if (await _userRepo.UserNameExists(user.UserName, user.Id))
                {
                    ModelState.AddModelError(string.Empty, "کاربر دیگری با همین نام در سیستم ثبت شده");
                    return View(user);
                }
                if (await _userRepo.EmailExists(user.Email, user.Id))
                {
                    ModelState.AddModelError(string.Empty, "کاربر دیگری با همین ایمیل در سیستم ثبت شده");
                    return View(user);
                }
                #endregion

                #region Upload Image
                if (UserAvatar != null)
                {
                    var imageName = await ImageHelper.SaveImage(UserAvatar, 400, 400, "UserAvatars", true);

                    user.Avatar = imageName;
                }
                #endregion
                var prevUser = await _userManager.FindByIdAsync(user.Id);
                prevUser.Email = user.Email;
                prevUser.FirstName = user.FirstName;
                prevUser.LastName = user.LastName;
                prevUser.UserName = user.UserName;
                prevUser.PhoneNumber = user.PhoneNumber;
                prevUser.Information = user.Information;
                prevUser.Avatar = user.Avatar;
                var result = await _userRepo.UpdateUser(prevUser);
                return RedirectToAction("Index", "Home");
            }

            return View(user);

        }
        [AllowAnonymous]
        public ActionResult ResetMyPassword(string id)
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IdentityResult> ResetMyPassword(ResetMyPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.FindByNameAsync(MyAppContext.Current.User.Identity.Name);
                var result = await _userManager.ChangePasswordAsync(currentUser, model.OldPassword, model.Password);
                return result;
            }
            return IdentityResult.Failed();
        }
        public async Task<IActionResult> EditRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);

            ViewBag.Email = user?.Email;
            ViewBag.UserId = user?.Id;

            var allRoles = _roleManager.Roles.ToList();
            var roles = allRoles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Selected = userRoles.Contains(x.Name)
            }).ToList();
            return View(roles);
        }
        [HttpPost]
        public async Task<IActionResult> EditRoles(string userId, List<RoleViewModel> roles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userId);
                var userRoles = await _userManager.GetRolesAsync(user);

                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRolesAsync(user, roles.Where(x => x.Selected).Select(x => x.Name));

                return RedirectToAction(nameof(Index));
            }

            return View(roles);
        }
        [HttpPost]
        public async Task<IdentityResult> ResetPasswordToDefault(string id)
        {
            var result = await _userRepo.ResetPasswordToDefault(id);
            return result;
        }
        public async Task<ActionResult> Delete(string id)
        {
            return PartialView(await _userRepo.GetById(id));
        }
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }
    }
}
