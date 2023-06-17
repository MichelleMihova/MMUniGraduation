using Microsoft.AspNetCore.Hosting;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Services.Interfaces;
using MMUniGraduation.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Services
{
    public class LectorService : ILectorService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif", "jpeg", "tif" };
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHost;
        public LectorService(ApplicationDbContext db, IWebHostEnvironment webHost)
        {
            _db = db;
            _webHost = webHost;
        }
        public async Task EditInfo(EditLectorViewModel input)
        {
            var currLector = _db.Lectors.FirstOrDefault(x => x.Id == input.Id);
            var lectorPhoto = _db.Images.FirstOrDefault(x => x.LectorId == currLector.Id);

            if (input.FirstName != null && input.FirstName != currLector.FirstName)
            {
                currLector.FirstName = input.FirstName;
            }

            if (input.LastName != null && input.LastName != currLector.LastName)
            {
                currLector.LastName = input.LastName;
            }

            if (input.Bio != currLector.Bio)
            {
                currLector.Bio = input.Bio;
            }

            if (lectorPhoto != null)
            {
                //add delete from folder
                _db.Images.Remove(lectorPhoto);

                await _db.SaveChangesAsync();
            }

            foreach (var photo in input.Photos)
            {
                var extension = Path.GetExtension(photo.FileName).TrimStart('.');
                var wwwrootPath = _webHost.WebRootPath;

                if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
                {
                    throw new Exception($"Invalid image extension {extension} !");
                }

                var dbImage = new Image
                {
                    Extension = extension,
                };

                currLector.Photos.Add(dbImage);

                var physicalPath = $"{wwwrootPath}/img/{dbImage.Id}.{extension}";
                await using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await photo.CopyToAsync(fileStream);
            }

            await _db.SaveChangesAsync();
        }

    }

}
