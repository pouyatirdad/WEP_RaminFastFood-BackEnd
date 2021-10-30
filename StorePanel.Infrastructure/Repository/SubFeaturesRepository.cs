using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;

namespace StorePanel.Infrastructure.Repository
{
    public interface ISubFeaturesRepository : IBaseRepository<SubFeature>
    {
        List<SubFeature> GetSubFeatures(int featureId);
        string GetFeatureName(int featureId);
    }

    public class SubFeaturesRepository : BaseRepository<SubFeature>,ISubFeaturesRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;
        public SubFeaturesRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
        public List<SubFeature> GetSubFeatures(int featureId)
        {
            return _context.SubFeatures.Where(h => h.FeatureId == featureId & h.IsDeleted == false).ToList();
        }
        public string GetFeatureName(int featureId)
        {
            return _context.Features.Find(featureId).Title;
        }
    }
}
