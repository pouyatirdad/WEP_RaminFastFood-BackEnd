using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using StorePanel.Infrastructure.Context;
using StorePanel.Web.Areas.Dashboard.ViewModels;
using StorePanel.Core.Models;

namespace StorePanel.Web.Helpers
{
    public interface IRolePermissionService
    {
        Task<bool> GetMenuItemsAsync(ClaimsPrincipal ctx, string ctrl, string act);
        Task<List<NavigationMenuViewModel>> GetMenuItemsAsync(ClaimsPrincipal principal);
        Task<List<NavigationMenuViewModel>> GetPermissionsByRoleIdAsync(string id);
        Task<bool> SetPermissionsByRoleIdAsync(string id, IEnumerable<int> permissionIds);

    }

    public class RolePermissionService : IRolePermissionService
    {
        private readonly StorePanelDbContext _context;
        private readonly IMemoryCache _cache;

        public RolePermissionService(StorePanelDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<List<NavigationMenuViewModel>> GetMenuItemsAsync(ClaimsPrincipal principal)
        {
            var isAuthenticated = principal.Identity.IsAuthenticated;
            if (!isAuthenticated)
            {
                return new List<NavigationMenuViewModel>();
            }

            var roleIds = await GetUserRoleIds(principal);

            var permissions = await _cache.GetOrCreateAsync("Permissions",
                async x => await (from menu in _context.NavigationMenu select menu).ToListAsync());

            var rolePermissions = await _cache.GetOrCreateAsync("RolePermissions",
                async x => await (from menu in _context.RoleMenuPermission select menu).Include(x => x.NavigationMenu).ToListAsync());

            var data = (from menu in rolePermissions
                        join p in permissions on menu.NavigationMenuId equals p.Id
                        where roleIds.Contains(menu.RoleId)
                        select p)
                              .Select(m => new NavigationMenuViewModel()
                              {
                                  Id = m.Id,
                                  Name = m.Name,
                                  Icon = m.Icon,
                                  ElementIdentifier = m.ElementIdentifier,
                                  Visible = m.Visible,
                                  ActionName = m.ActionName,
                                  DisplayOrder = m.DisplayOrder,
                                  ParentMenuId = m.ParentMenuId,
                                  ControllerName = m.ControllerName,
                              }).Distinct().ToList();

            return data;
        }

        public async Task<bool> GetMenuItemsAsync(ClaimsPrincipal ctx, string ctrl, string act)
        {
            var result = false;
            var roleIds = await GetUserRoleIds(ctx);
            var data = await (from menu in _context.RoleMenuPermission
                              where roleIds.Contains(menu.RoleId)
                              select menu)
                              .Select(m => m.NavigationMenu)
                              .Distinct()
                              .ToListAsync();

            foreach (var item in data)
            {
                result = (item.ControllerName == ctrl && item.ActionName == act);
                if (result)
                {
                    break;
                }
            }

            return result;
        }

        public async Task<List<NavigationMenuViewModel>> GetPermissionsByRoleIdAsync(string id)
        {
            var items = await (from m in _context.NavigationMenu
                               join rm in _context.RoleMenuPermission
                                on new { X1 = m.Id, X2 = id } equals new { X1 = rm.NavigationMenuId, X2 = rm.RoleId }
                                into rmp
                               from rm in rmp.DefaultIfEmpty()
                               select new NavigationMenuViewModel()
                               {
                                   Id = m.Id,
                                   Name = m.Name,
                                   Icon = m.Icon,
                                   DisplayOrder = m.DisplayOrder,
                                   ElementIdentifier = m.ElementIdentifier,
                                   ActionName = m.ActionName,
                                   ControllerName = m.ControllerName,
                                   //DisplayOrder = m.DisplayOrder,
                                   ParentMenuId = m.ParentMenuId,
                                   Visible = m.Visible,
                                   Permitted = rm.RoleId == id
                               })
                               .AsNoTracking()
                               .ToListAsync();

            return items;
        }

        public async Task<bool> SetPermissionsByRoleIdAsync(string id, IEnumerable<int> permissionIds)
        {
            var existing = await _context.RoleMenuPermission.Where(x => x.RoleId == id).ToListAsync();
            _context.RemoveRange(existing);

            foreach (var item in permissionIds)
            {
                await _context.RoleMenuPermission.AddAsync(new RoleMenuPermission()
                {
                    RoleId = id,
                    NavigationMenuId = item,
                });
            }

            var result = await _context.SaveChangesAsync();

            // Remove existing permissions to roles so it can re evaluate and take effect
            _cache.Remove("RolePermissions");

            return result > 0;
        }

        private async Task<List<string>> GetUserRoleIds(ClaimsPrincipal ctx)
        {
            var userId = GetUserId(ctx);
            var data = await (from role in _context.UserRoles
                              where role.UserId == userId
                              select role.RoleId).ToListAsync();

            return data;
        }

        private string GetUserId(ClaimsPrincipal user)
        {
            return ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }

}
