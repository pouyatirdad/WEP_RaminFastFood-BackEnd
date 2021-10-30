using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Core.Models
{
    public class ProductGroupFeature : BaseEntity
    {
        public int ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; }
        public int FeatureId { get; set; }
        public Feature Feature { get; set; }
    }
}
