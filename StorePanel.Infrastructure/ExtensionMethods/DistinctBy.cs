using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorePanel.Infrastructure.ExtensionMethods
{
    public static class DistinctByExtension
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            return items.GroupBy(property).Select(x => x.First());
        }
    }
}
