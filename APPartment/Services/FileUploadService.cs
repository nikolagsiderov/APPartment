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
        private HumanSizeConverter humanSizeConverter = new HumanSizeConverter();

        public void UploadImage(IFormFile file, DataAccessContext _context, long targetId)
        {
            var imageName = SaveImageToDB(file, _context, targetId);

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

        private string SaveImageToDB(IFormFile file, DataAccessContext _context, long targetId)
        {
            var image = new Image()
            {
                FileName = file.FileName,
                FileSize = humanSizeConverter.ConvertFileLength(file),
                CreatedDate = DateTime.Now,
                TargetId = targetId,
            };

            _context.Add(image);
            _context.SaveChanges();

            image.Name = $"{image.Id}_{targetId}_{file.FileName}";

            _context.Update(image);
            _context.SaveChanges();

            return image.Name;
        }
    }
}
