using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Infrastructure.Helpers
{
    public static class ImageHelper
    {
        public static async Task<string> SaveImage(IFormFile file)
        {

            var tempFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Temp");
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var tempFilePath = Path.Combine(tempFolder, fileName);

            await using (var fileStream = new FileStream(tempFolder, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Images");
            var uploadFilePath = Path.Combine(uploadFolder, fileName);

            ImageResizer resizer = new ImageResizer();
            resizer.Resize(tempFilePath, uploadFilePath);

            return fileName;
        }
        public static async Task<string> SaveImage(IFormFile file, int x, int y)
        {

            var tempFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Temp");
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var tempFilePath = Path.Combine(tempFolder, fileName);

            await using (var fileStream = new FileStream(tempFolder, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Images");
            var uploadFilePath = Path.Combine(uploadFolder, fileName);

            ImageResizer resizer = new ImageResizer(x, y);
            resizer.Resize(tempFilePath, uploadFilePath);

            return fileName;
        }
        public static async Task<string> SaveImage(IFormFile file, int x, int y, bool trim)
        {

            var tempFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Temp");
            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var tempFilePath = Path.Combine(tempFolder, fileName);

            await using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Images");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var uploadFilePath = Path.Combine(uploadFolder, fileName);

            ImageResizer resizer = new ImageResizer(x, y, trim);
            resizer.Resize(tempFilePath, uploadFilePath);

            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);

            return fileName;
        }
        public static async Task<string> SaveImage(IFormFile file, int x, int y, string folder, bool trim)
        {

            var tempFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Temp");
            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var tempFilePath = Path.Combine(tempFolder, fileName);

            await using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", folder);

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var uploadFilePath = Path.Combine(uploadFolder, fileName);

            ImageResizer resizer = new ImageResizer(x, y, trim);
            resizer.Resize(tempFilePath, uploadFilePath);

            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);

            return fileName;
        }
    }
}
