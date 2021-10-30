using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.ViewComponents
{
    public class FooterDataViewComponent : ViewComponent
    {
        private readonly IStaticContentRepository _staticContentRepository;

        public FooterDataViewComponent(IStaticContentRepository staticContentRepository)
        {
            _staticContentRepository = staticContentRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _staticContentRepository.GetById(1));
        }
    }
}
