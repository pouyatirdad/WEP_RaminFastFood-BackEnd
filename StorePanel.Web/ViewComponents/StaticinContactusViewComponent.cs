using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.ViewComponents
{
    public class StaticinContactusViewComponent : ViewComponent
    {
        private readonly IStaticContentRepository _staticContentRepository;

        public StaticinContactusViewComponent(IStaticContentRepository staticContentRepository)
        {
            _staticContentRepository = staticContentRepository;
        }

        public IViewComponentResult Invoke()
        {
            return View(_staticContentRepository.GetAll().Result[0]);
        }
    }
}
