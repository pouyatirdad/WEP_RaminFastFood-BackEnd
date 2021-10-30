using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StorePanel.Web.Helpers
{
    public static class SeoHelper
    {
        public static string Slugify(this string str)
        {
            if (str == null) { return null; }

            return str.ToLower().Replace('_', ' ').Replace(' ', '-');
        }
    }
}
