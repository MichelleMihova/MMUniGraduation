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
    public class StudentService: IStudentService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif", "jpeg", "tif" };
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHost;
        public StudentService(ApplicationDbContext db, IWebHostEnvironment webHost)
        {
            _db = db;
            _webHost = webHost;
        }

        public async Task EditStudent(EditStudentViewModel input)
        {
            var currStudent = _db.Students.FirstOrDefault(x => x.Id == input.Id);
            var studentPhoto = _db.Images.FirstOrDefault(x => x.UserId == currStudent.Id);

            if (input.FirstName != null && input.FirstName != currStudent.FirstName)
            {
                currStudent.FirstName = input.FirstName;
            }

            if (input.LastName != null && input.LastName != currStudent.LastName)
            {
                currStudent.LastName = input.LastName;
            }

            if (input.ShowTextMaterials != currStudent.ShowTextMaterials)
            {
                currStudent.ShowTextMaterials = input.ShowTextMaterials;
            }

            if (input.ShowVideoMaterials != currStudent.ShowVideoMaterials)
            {
                currStudent.ShowVideoMaterials = input.ShowVideoMaterials;
            }


            if (studentPhoto != null)
            {
                //add delete from folder
                _db.Images.Remove(studentPhoto);

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

                currStudent.Photos.Add(dbImage);

                var physicalPath = $"{wwwrootPath}/img/{dbImage.Id}.{extension}";
                await using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await photo.CopyToAsync(fileStream);
            }

            await _db.SaveChangesAsync();
        }

    }
}
