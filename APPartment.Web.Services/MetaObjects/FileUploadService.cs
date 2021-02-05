using APPartment.Data.Core;
using APPartment.Data.Server.Models.MetaObjects;
using APPartment.ORM.Framework.Core;
using APPartment.Web.Services.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace APPartment.Web.Services.MetaObjects
{
    public class FileUploadService
    {
        private DataAccessContext _context;
        private DataContext dataContext;
        private HumanSizeConverter humanSizeConverter = new HumanSizeConverter();

        public FileUploadService(DataAccessContext _context, DataContext dataContext)
        {
            this._context = _context;
            this.dataContext = dataContext;
        }

        public void UploadImage(IFormFile file, long targetId, long? userId, long? homeId)
        {
            var imageName = SaveImageToDB(file, targetId, (long)userId, (long)homeId);

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

        private string SaveImageToDB(IFormFile file, long targetId, long userId, long homeId)
        {
            var image = new Image()
            {
                FileName = file.FileName,
                FileSize = humanSizeConverter.ConvertFileLength(file),
                CreatedDate = DateTime.Now,
                TargetId = targetId,
            };

            Task.Run(() => dataContext.SaveAsync(image, userId, homeId, targetId)).Wait();

            image.Name = $"{image.Id}_{targetId}_{file.FileName}";

            _context.Update(image);
            _context.SaveChanges();

            return image.Name;
        }
    }
}
