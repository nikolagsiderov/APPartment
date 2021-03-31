using APPartment.UI.Html;
using APPartment.UI.Services.Base;
using APPartment.UI.ViewModels.Clingons.Image;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace APPartment.UI.Services
{
    public class FileUploadService
    {
        private BaseWebService BaseWebService;

        public FileUploadService(long? currentUserID)
        {
            this.BaseWebService = new BaseWebService(currentUserID);
        }

        public void UploadImage(IFormFile file, long targetObjectID, long currentUserID)
        {
            var imageName = SaveImageToDB(file, targetObjectID, currentUserID);
            string pathString = "wwwroot\\BaseObject_Images";
            bool isExists = System.IO.Directory.Exists(pathString);

            if (!isExists)
                Directory.CreateDirectory(pathString);

            var path = string.Format($"{pathString}\\{imageName}");

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }

        private string SaveImageToDB(IFormFile file, long targetObjectID, long currentUserID)
        {
            var image = new ImagePostViewModel()
            {
                Name = file.FileName,
                FileSize = HumanSizeConverter.ConvertFileLength(file),
                CreatedDate = DateTime.Now,
                TargetObjectID = targetObjectID,
            };

            // We save the image once
            // Then we save again with modified Name property to add its ID
            BaseWebService.Save(image);
            image.Name = $"{image.ID}_{targetObjectID}_{file.FileName}";
            BaseWebService.Save(image);

            return image.Name;
        }
    }
}
