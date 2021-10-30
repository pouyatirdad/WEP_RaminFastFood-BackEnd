using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.TagHelpers
{
    [HtmlTargetElement("validate-permission")]
    public class ValidatePermissionTagHelper : TagHelper
    {
        private readonly StorePanelDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public ValidatePermissionTagHelper(StorePanelDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        [HtmlAttributeName("asp-area")]
        public string Area { get; set; } = "Dashboard";

        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            var user = _httpContextAccessor.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                output.SuppressOutput();
                return;
            }
            var usr = await _userManager.GetUserAsync(user);
            var userRoles = _dbContext.UserRoles.Where(ur => ur.UserId == usr.Id).Select(ur => ur.RoleId).ToList();
            var userHasPermission = _dbContext.RoleMenuPermission
                .Any(rp => userRoles.Any(ur => ur == rp.RoleId)
                && rp.NavigationMenu.ControllerName.Trim().ToLower().Equals(Controller)
                && rp.NavigationMenu.ActionName.Trim().ToLower().Equals(Action));


            if (userHasPermission == true)
                return;

            output.SuppressOutput();
        }
    }
}
