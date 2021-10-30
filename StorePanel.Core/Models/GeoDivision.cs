using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Core.Models
{
    public class GeoDivision : BaseEntity
    {
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string LatinTitle { get; set; }

        public int GeoDivisionType { get; set; }
        public int? IsLeaf { get; set; }
        public int? IsArchived { get; set; }
        public int? ParentId { get; set; }
        public virtual GeoDivision Parent { get; set; }
        public virtual ICollection<GeoDivision> Children { get; set; }
    }
}
