using APPartment.Infrastructure.Services.Base;
using APPartment.Infrastructure.Tools;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Image;
using APPartment.Common;
using System;
using System.IO;

namespace APPartment.Infrastructure.Services
{
    public class FileService : BaseCRUDService
    {
        public FileService(long? currentUserID, long? currentHomeID) : base(currentUserID, currentHomeID)
        {
        }

        public void UploadImage(byte[] fileBytes, long targetObjectID)
        {
            var imageName = SaveImageToDB(fileBytes, targetObjectID);
            bool isExists = Directory.Exists(Configuration.ImagesPath);

            if (!isExists)
                Directory.CreateDirectory(Configuration.ImagesPath);

            var path = string.Format($"{Configuration.ImagesPath}\\{imageName}");

            File.WriteAllBytes(path, fileBytes);
        }

        private string SaveImageToDB(byte[] fileBytes, long targetObjectID)
        {
            var image = new ImagePostViewModel()
            {
                Name = "temp",
                FileSize = HumanSizeConverter.ConvertFileLength(fileBytes),
                CreatedDate = DateTime.Now,
                TargetObjectID = targetObjectID,
            };

            // We save the image once
            // Then we save again with modified Name property to add its ID
            image = Save(image);
            image.Name = $"{image.ID}_{targetObjectID}.png";
            image = Save(image);
            AddUserAsParticipantToObjectIfNecessary(image.TargetObjectID, image.CreatedByID);

            return image.Name;
        }

        public void DeleteImage(long ID)
        {
            var image = GetEntity<ImagePostViewModel>(ID);

            if (Directory.Exists(Configuration.ImagesPath))
            {
                var di = new DirectoryInfo(Configuration.ImagesPath);
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
