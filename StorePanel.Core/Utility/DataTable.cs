using System;
using System.Collections.Generic;
using System.Text;

namespace StorePanel.Core.Utility
{
    public enum DataTableOrder
    {
        Ascending,
        Descending
    }
    public class DataTableModel
    {
        public string Url { get; set; }
        public int OrderBy { get; set; } = 0;
        public string Order { get; set; } = "desc";
        public List<DataTableColumn> Columns { get; set; }

    }
    public class DataTableColumn
    {
        public string Identifier { get; set; }
        private string name;

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    return Identifier;
                }
                else
                {
                    return name;
                };
            }
            set { name = value; }
        }
        public string Render { get; set; }
    }
}
