using DataTablesParser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StorePanel.Core.Models;
using StorePanel.Web.Areas.Dashboard.ViewModels;
using StorePanel.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize("Permission")]
    public class RolesController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(
                UserManager<User> userManager,
                RoleManager<IdentityRole> roleManager,
                IRolePermissionService rolePermissionService,
                ILogger<RolesController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _rolePermissionService = rolePermissionService;
            _logger = logger;
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
            var roles = _roleManager.Roles.Select(item =>
                            new RoleViewModel()
                            {
                                Id = item.Id,
                                Name = item.Name,
                            }
                            );
            var parser = new Parser<RoleViewModel>(Request.Form, roles);
            return JsonConvert.SerializeObject(parser.Parse());
        }
        public IActionResult Create()
        {
            return PartialView(new RoleViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole() { Name = viewModel.Name });
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", string.Join(",", result.Errors));
                }
            }

            return View(viewModel);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var vm = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            return PartialView(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(vm.Id);
                role.Name = vm.Name;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", string.Join(",", result.Errors));
                }
            }

            return View(vm);
        }
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return PartialView(role);
        }
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var result = await _roleManager.DeleteAsync(role);

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> EditRolePermission(string id)
        {
            var permissions = new List<NavigationMenuViewModel>();
            if (!string.IsNullOrWhiteSpace(id))
            {
                permissions = await _rolePermissionService.GetPermissionsByRoleIdAsync(id);
            }
            var role = await _roleManager.FindByIdAsync(id);
            ViewBag.RoleName = role.Name;
            ViewBag.RoleId = id;
            return View(permissions);
        }

        [HttpPost]
        public async Task<IActionResult> EditRolePermission(string id, string selectedPermissions)
        {
            if (ModelState.IsValid)
            {
                var permissionIds = new List<int>();
                if (selectedPermissions != null && string.IsNullOrEmpty(selectedPermissions) == false)
                {
                    var selectedPermisisonsArr = selectedPermissions.Split(',');
                    permissionIds = selectedPermisisonsArr.Select(x => Convert.ToInt32(x)).ToList();
                }
                await _rolePermissionService.SetPermissionsByRoleIdAsync(id, permissionIds);
                return RedirectToAction(nameof(Index));
            }
            var role = await _roleManager.FindByIdAsync(id);
            ViewBag.RoleName = role.Name;
            ViewBag.RoleId = id;
            return View(selectedPermissions);
        }
    }
}
