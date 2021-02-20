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

        public void UploadImage(IFormFile file, long targetObjectId)
        {
            var imageName = SaveImageToDB(file, targetObjectId);
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

        private string SaveImageToDB(IFormFile file, long targetObjectId)
        {
            var image = new Image()
            {
                Name = file.FileName,
                FileSize = humanSizeConverter.ConvertFileLength(file),
                CreatedDate = DateTime.Now,
                TargetObjectId = targetObjectId,
            };

            baseFacade.Create(image);

            image.Name = $"{image.Id}_{targetObjectId}_{file.FileName}";

            baseFacade.Update(image);

            return image.Name;
        }
    }
}
