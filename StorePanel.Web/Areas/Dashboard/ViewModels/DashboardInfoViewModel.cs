using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Areas.Dashboard.ViewModels
{
    public class DashboardInfoViewModel
    {
        public int CustomerCount { get; set; }

        public int InvoiceCount { get; set; }

        public int ProductCount { get; set; }

        public long TotalSell { get; set; }
    }
}
