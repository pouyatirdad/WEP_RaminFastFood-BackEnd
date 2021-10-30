using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Repository;
using StorePanel.Web.Models;
using StorePanel.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace StorePanel.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AuthController : Controller
    {
        private UserManager<User> _userManager;

        private SignInManager<User> _signInManager;

        private readonly ICustomersRepository _customersRepository;

        private readonly IUserRepository _userRepository;

        public AuthController(UserManager<User> userManager
        ,SignInManager<User> signInManager
        ,ICustomersRepository customersRepository
        ,IUserRepository userRepository)
        {
            _userManager = userManager;

            _signInManager = signInManager;

            _customersRepository = customersRepository;

            _userRepository = userRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(StorePanel.Web.Models.RegisterCustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _userRepository.EmailExists(model.Email))
                {
                    ViewBag.RegisterError = "ایمیل قبلا ثبت شده.";
                    //ModelState.AddModelError("", "ایمیل قبلا ثبت شده");
                    return View(model);
                }

                var user = new User { UserName = model.Email, Email = model.Email};

                var result = await _userManager.CreateAsync(user, model.Password);

                await _userManager.AddToRoleAsync(user, "Customer");

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);

                    StorePanel.Core.Models.Customer customer = new StorePanel.Core.Models.Customer()
                    {
                        UserId = user.Id,
                        IsDeleted = false,
                        InsertDate = DateTime.Now,
                        
                    };

                    await _customersRepository.Add(customer);

                    return RedirectToAction("Index", "Dashboard", new { area = "Customer" });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(model.Username);

                bool IsinRole = await _userManager.IsInRoleAsync(user, "Customer");

                if (IsinRole)
                {
                    if (await _userManager.CheckPasswordAsync(user, model.Password) == false)
                    {

                        ModelState.AddModelError("message", "Invalid credentials");

                        return View(model);
                    }

                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return Redirect("/Customer/Dashboard/Index");
                    }
                    else if (result.IsLockedOut)
                    {
                        return View("AccountLocked");
                    }
                    else
                    {
                        ModelState.AddModelError("message", "Invalid login attempt");

                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "access denied");

                    return View(model);
                }

            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home",new { area=""});
        }
    }
}
