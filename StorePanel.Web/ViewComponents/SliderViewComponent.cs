using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.ViewComponents
{
    public class SliderViewComponent :ViewComponent
    {
        private readonly ISliderRepository _sliderRepository;

        public SliderViewComponent(ISliderRepository sliderRepository)
        {
            _sliderRepository = sliderRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _sliderRepository.GetAll());
        }
    }
}
