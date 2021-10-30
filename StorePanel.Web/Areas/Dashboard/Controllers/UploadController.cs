using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Areas.Dashboard.Controllers
{
    public class UploadController : Controller
    {
        [Route("Dashboard/Upload/Video")]
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
        ValueLengthLimit = int.MaxValue)]
        public async Task<IActionResult> Index(IFormFile file)
        {
            try
            {
                if (file == null || file.Length <= 0)
                    return View();

                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Videos");
                var fileName = "id" + "-(" + DateTime.Now.ToString("yyyy-M-d-hhmmss") + ")" + Path.GetExtension(file.FileName);
                var finalPath = Path.Combine(uploadFolder, fileName);

                await using (var fileStream = new FileStream(finalPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return Ok(new { Message = "UploadDone" });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = e.ToString() + "UploadFailed" }); ;
            }
        }
    }
}
