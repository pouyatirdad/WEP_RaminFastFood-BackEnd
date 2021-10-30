using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StorePanel.Core.Models
{
    [Table(name: "AspNetRoleMenuPermission")]
    public class RoleMenuPermission
    {
        public string RoleId { get; set; }

        
        public int NavigationMenuId { get; set; }

        public NavigationMenu NavigationMenu { get; set; }
    }
}
