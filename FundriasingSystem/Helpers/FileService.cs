using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace FundraisingApp.Helpers
{
    public class FileService
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public FileService(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        public string FileUpload(IFormFile file)
        {
            string uniFileName = null;
            if (file != null)
            {
                string uploadFoler = Path.Combine(_hostEnvironment.WebRootPath, "img");
                uniFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadFoler, uniFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            return uniFileName;
        }

        public string UpdateFile(string existingPath, IFormFile file)
        {
            string uniFileName = null;
            if (file != null)
            {
                if(existingPath != null)
                {
                    string existingFilePath = Path.Combine(_hostEnvironment.WebRootPath, "img", existingPath);
                    File.Delete(existingFilePath);
                }

                uniFileName = FileUpload(file);
            }

            return uniFileName;
        }
    }
}
