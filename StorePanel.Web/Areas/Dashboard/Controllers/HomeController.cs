using ImageMagick;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Route("Dashboard")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ITagRepository _tagRepo;

        private UserManager<User> _userManager;

        public HomeController(ITagRepository tagRepo,UserManager<User> userManager)
        {
            _tagRepo = tagRepo;

            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }
            ViewBag.Avatar = currentUser.Avatar;
            return View();
        }

        [AllowAnonymous]
        [Route("Home/GetTags")]
        public JsonResult GetTags(string searchStr)
        {
            var suggestion = new List<string>();
            if (string.IsNullOrEmpty(searchStr) == false)
            {
                suggestion = _tagRepo.GetDefaultQuery().Where(
                    p => p.Title.ToLower().Trim().Contains(searchStr.ToLower().Trim())
                ).Select(p => p.Title).ToList();
            }
            return Json(suggestion);
        }
        [AllowAnonymous]
        [Route("Home/UploadImage")]
        public async Task<string> UploadImage(IFormFile file, bool? hasWatermark)
        {
            var fileName = Guid.NewGuid() + ".webp";
            var tempUploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Temp");
            var tempUploadFilePath = Path.Combine(tempUploadFolder, fileName);

            await using (var fileStream = new FileStream(tempUploadFilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);

                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Images");
                var uploadFilePath = Path.Combine(uploadFolder, fileName);
                var waterMark = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "light-logo.png");
                using (MagickImage image = new MagickImage(tempUploadFilePath))
                {
                    if (hasWatermark != null && hasWatermark == true)
                    {
                        using (var watermark = new MagickImage(waterMark))
                        {

                            watermark.Evaluate(Channels.Alpha, EvaluateOperator.Divide, 4);
                            image.Composite(watermark, Gravity.Southeast, CompositeOperator.Over);

                        }
                    }
                    image.Format = MagickFormat.WebP;
                    image.Quality = 75;
                    image.Write(uploadFilePath);
                }

                var lowQualityFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "LowImage");
                var lowQualityFilePath = Path.Combine(lowQualityFolder, fileName);
                using (MagickImage image = new MagickImage(tempUploadFilePath))
                {
                    image.Blur(0, 10);
                    image.Format = MagickFormat.WebP;
                    image.Quality = 10;
                    image.Write(lowQualityFilePath);
                }
            }
            if (System.IO.File.Exists(tempUploadFilePath))
            {
                try
                {
                    System.IO.File.Delete(tempUploadFilePath);
                }
                catch (Exception ex)
                {
                    //Do something
                }
            }
            return fileName;
        }
    }
}
