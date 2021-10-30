using Microsoft.AspNetCore.Mvc;
using StorePanel.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace StorePanel.Web.Areas.Dashboard.Components
{
	public class DataTableViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(DataTableModel model) {

            return View(model);
        }
    }
}
