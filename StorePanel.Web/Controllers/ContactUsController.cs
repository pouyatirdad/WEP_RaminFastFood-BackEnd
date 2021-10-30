using Microsoft.AspNetCore.Mvc;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IContactUsFormRepository _contactUsFormRepository;

        private readonly IStaticContentRepository _staticContentRepository;

        public ContactUsController(IContactUsFormRepository contactUsFormRepository
        ,IStaticContentRepository staticContentRepository)
        {
            _contactUsFormRepository = contactUsFormRepository;

            _staticContentRepository = staticContentRepository;
        }

        public async Task<IActionResult> Index(bool Saved=false)
        {
            ViewBag.Saved = Saved;
            return View(await _staticContentRepository.GetById(1));
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactForm model)
        {
            if (ModelState.IsValid)
            {
                await _contactUsFormRepository.Add(model);
            }

            return RedirectToAction("Index",new { Saved=true});
        }

        public async Task<IActionResult> News(ContactForm contactForm)
        {
            contactForm.Message = "عضویت خبرنامه";
            contactForm.Phone = "-";
            contactForm.Name = "-";
            await _contactUsFormRepository.Add(contactForm);

            return RedirectToAction("Index","Home");
        }
        public IActionResult Subscribe()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Subscribe(ContactForm model)
        {

            if (ModelState.IsValid)
            {
                _contactUsFormRepository.Add(model);
            }

            return PartialView(model);
        }
    }
}
