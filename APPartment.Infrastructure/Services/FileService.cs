using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.Tools;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Image;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace APPartment.Infrastructure.Services
{
    public class FileService : BaseCRUDService
    {
        public FileService(long? currentUserID, long? currentHomeID) : base(currentUserID, currentHomeID)
        {
        }

        public void UploadImage(IFormFile file, long targetObjectID, long currentUserID)
        {
            var imageName = SaveImageToDB(file, targetObjectID, currentUserID);
            string pathString = "wwwroot\\BaseObject_Images";
            bool isExists = Directory.Exists(pathString);

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
            Save(image);
            image.Name = $"{image.ID}_{targetObjectID}_{file.FileName}";
            Save(image);

            return image.Name;
        }
    }
}
