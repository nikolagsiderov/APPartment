using APPartment.UI.Services.Base;
using APPartment.UI.Utilities;
using APPartment.UI.ViewModels.Image;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace APPartment.UI.Services
{
    public class FileUploadService
    {
        private BaseWebService BaseWebService;
        private HumanSizeConverter humanSizeConverter;

        public FileUploadService(long? currentUserId)
        {
            this.BaseWebService = new BaseWebService(currentUserId);
            this.humanSizeConverter = new HumanSizeConverter();
        }

        public void UploadImage(IFormFile file, long targetObjectId, long currentUserId)
        {
            var imageName = SaveImageToDB(file, targetObjectId, currentUserId);
            string pathString = "wwwroot\\BaseObject_Images";
            bool isExists = System.IO.Directory.Exists(pathString);

            if (!isExists)
                System.IO.Directory.CreateDirectory(pathString);

            var path = string.Format($"{pathString}\\{imageName}");

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }

        private string SaveImageToDB(IFormFile file, long targetObjectId, long currentUserId)
        {
            var image = new ImagePostViewModel()
            {
                Name = file.FileName,
                FileSize = humanSizeConverter.ConvertFileLength(file),
                CreatedDate = DateTime.Now,
                TargetObjectId = targetObjectId,
            };

            BaseWebService.Save(image);

            image.Name = $"{image.Id}_{targetObjectId}_{file.FileName}";

            BaseWebService.Save(image);

            return image.Name;
        }
    }
}
