using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StorePanel.Core.Models
{
    [Table(name: "AspNetNavigationMenu")]
    public class NavigationMenu
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("ParentNavigationMenu")]
        public int? ParentMenuId { get; set; }

        public virtual NavigationMenu ParentNavigationMenu { get; set; }

        public string Icon { get; set; }
        public int? DisplayOrder { get; set; }
        public string ElementIdentifier { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        [NotMapped]
        public bool Permitted { get; set; }

        public bool Visible { get; set; }
    }
}
