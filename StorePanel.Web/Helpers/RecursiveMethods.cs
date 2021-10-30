using StorePanel.Web.Areas.Dashboard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Helpers
{
    public static class RecursiveMethods
    {
        public static string GetPermissionsTree(List<NavigationMenuViewModel> list, NavigationMenuViewModel item)
        {
            var children = list.Where(l =>l.ParentMenuId == item.Id).ToList();
            var content = "<li data-jstree='{ \"selected\" : "+ item.Permitted.ToString().ToLower() + ",\"opened\": true }' id=\"" + item.Id + "\">"+item.Name;

            if (children.Any())
            {
                children.ForEach(child =>
                {
                        content += "<ul>" + GetPermissionsTree(list, child) + "</ul>";
                });
            }
            content += "</li>";

            return content;
        }
    }
}
