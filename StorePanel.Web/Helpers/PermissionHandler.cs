﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Helpers
{
    public class AuthorizationRequirement : IAuthorizationRequirement
    {
        public AuthorizationRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }

        public string PermissionName { get; }
    }

    public class PermissionHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        private readonly IRolePermissionService _rolePermissionService;

        public PermissionHandler(IRolePermissionService rolePermissionService)
        {
            _rolePermissionService = rolePermissionService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
        {
            if (context.Resource is RouteEndpoint endpoint)
            {
                endpoint.RoutePattern.RequiredValues.TryGetValue("controller", out var _controller);
                endpoint.RoutePattern.RequiredValues.TryGetValue("action", out var _action);

                endpoint.RoutePattern.RequiredValues.TryGetValue("page", out var _page);
                endpoint.RoutePattern.RequiredValues.TryGetValue("area", out var _area);

                // Check if a parent action is permitted then it'll allow child without checking for child permissions
                if (!string.IsNullOrWhiteSpace(requirement?.PermissionName) && !requirement.PermissionName.Equals("Permission"))
                {
                    _action = requirement.PermissionName;
                }

                if (context.User.Identity.IsAuthenticated && _controller != null && _action != null &&
                    await _rolePermissionService.GetMenuItemsAsync(context.User, _controller.ToString(), _action.ToString()))
                {
                    context.Succeed(requirement);
                }
            }

            await Task.CompletedTask;
        }
    }

}
