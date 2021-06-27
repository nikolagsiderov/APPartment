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
        private string imagesFileSharePath = @"C:\APPartmentFileShare\Images";

        public FileService(long? currentUserID, long? currentHomeID) : base(currentUserID, currentHomeID)
        {
        }

        public void UploadImage(IFormFile file, long targetObjectID)
        {
            var imageName = SaveImageToDB(file, targetObjectID);
            bool isExists = Directory.Exists(imagesFileSharePath);

            if (!isExists)
                Directory.CreateDirectory(imagesFileSharePath);

            var path = string.Format($"{imagesFileSharePath}\\{imageName}");

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }

        private string SaveImageToDB(IFormFile file, long targetObjectID)
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

        public void DeleteImage(long ID)
        {
            var image = GetEntity<ImagePostViewModel>(ID);

            if (Directory.Exists(imagesFileSharePath))
            {
                var di = new DirectoryInfo(imagesFileSharePath);
                foreach (var file in di.GetFiles())
                {
                    if (file.Name == image.Name)
                    {
                        file.Delete();
                        break;
                    }
                }

                this.Delete(image);
            }
        }
    }
}
