using APPartment.Data.Core;
using APPartment.Data.Server.Models.MetaObjects;
using APPartment.Web.Services.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace APPartment.Web.Services.MetaObjects
{
    public class FileUploadService
    {
        private readonly BaseFacade baseFacade;
        private HumanSizeConverter humanSizeConverter = new HumanSizeConverter();

        public FileUploadService()
        {
            this.baseFacade = new BaseFacade();
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
            var image = new Image()
            {
                Name = file.FileName,
                FileSize = humanSizeConverter.ConvertFileLength(file),
                CreatedDate = DateTime.Now,
                TargetObjectId = targetObjectId,
            };

            baseFacade.Create(image, currentUserId);

            image.Name = $"{image.Id}_{targetObjectId}_{file.FileName}";

            baseFacade.Update(image, currentUserId);

            return image.Name;
        }
    }
}
