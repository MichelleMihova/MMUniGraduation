using Microsoft.AspNetCore.Hosting;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using SmartProfil.AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Services
{
    public class StudyProgramService : IStudyProgramService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif", "jpeg", "tif" };

        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHost;

        public StudyProgramService(ApplicationDbContext db, IWebHostEnvironment webHost)
        {
            _db = db;
            _webHost = webHost;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this._db.StudyPrograms.Select(x => new
            {
                x.Id,
                x.Name,
            })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }

        public IEnumerable<T> GetPrograms<T>(int count)
        {
            return _db.StudyPrograms
                .OrderBy(x => Guid.NewGuid())
                .Take(count)
                .To<T>()
                .ToList();
        }

        public async Task CreateAsync(CreateStudyProgram model)
        {
            var studyProgram = new StudyProgram
            {
                Name = model.Name,
                Description = model.Description,
            };

            foreach (var image in model.Images)
            {
                var extension = Path.GetExtension(image.FileName).TrimStart('.');
                var wwwrootPath = _webHost.WebRootPath;

                if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
                {
                    throw new Exception($"Invalid image extension {extension} !");
                }

                var dbImage = new Image
                {
                    Extension = extension,
                    //StudyProgramId = studyProgram.Id,
                    //StudyProgram = studyProgram
                };

                studyProgram.Images.Add(dbImage);

                var physicalPath = $"{wwwrootPath}/img/{dbImage.Id}.{extension}";
                await using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await image.CopyToAsync(fileStream);
            }

            await _db.StudyPrograms.AddAsync(studyProgram);
            await _db.SaveChangesAsync();
        }
    }
}
