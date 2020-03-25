using APPartment.Core;
using APPartment.Data;
using APPartment.Models;
using APPartment.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace APPartment.Services
{
    public class FileUploadService
    {
        private DataAccessContext _context;
        private DataContext<Image> context;
        private HumanSizeConverter humanSizeConverter = new HumanSizeConverter();

        public FileUploadService(DataAccessContext _context, DataContext<Image> context)
        {
            this._context = _context;
            this.context = context;
        }

        public void UploadImage(IFormFile file, long targetId, long userId, long houseId)
        {
            var imageName = SaveImageToDB(file, targetId, userId, houseId);

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

        private string SaveImageToDB(IFormFile file, long targetId, long userId, long houseId)
        {
            var image = new Image()
            {
                FileName = file.FileName,
                FileSize = humanSizeConverter.ConvertFileLength(file),
                CreatedDate = DateTime.Now,
                TargetId = targetId,
            };

            context.Save(image, userId, targetId, houseId);

            image.Name = $"{image.Id}_{targetId}_{file.FileName}";

            _context.Update(image);
            _context.SaveChanges();

            return image.Name;
        }
    }
}
