using Microsoft.AspNetCore.Hosting;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Services
{
    public class LectureService : ILectureService
    {
        private readonly string[] allowedExtensions = new[] { "doc", "docx", "txt", "pptx", "pptm", "pdf" };

        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHost;
        public LectureService(ApplicationDbContext db, IWebHostEnvironment webHost)
        {
            _db = db;
            _webHost = webHost;
        }
        public async Task CreateLectureAsync(CreateLecture input)
        {
            var lecture = new Lecture
            {
                Name = input.Name,
                Description = input.Description,
                CourseId = input.CourseId,
                ParetntLectureId = input.ParetntLectureId,
                DateTimeToShow = input.DateTimeToShow,
                EndDateTimeForHW = input.EndDateTimeForHW,
            };

            foreach (var file in input.Files)
            {
                var extension = Path.GetExtension(file.FileName).TrimStart('.');
                var wwwrootPath = _webHost.WebRootPath;

                if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
                {
                    throw new Exception($"Invalid file extension {extension} !");
                }

                var lectureFile = new LectureFile
                {
                    Extension = extension,
                    FileName = file.FileName
                };

                lecture.TextMaterials.Add(lectureFile);

                var physicalPath = $"{wwwrootPath}/files/{lectureFile.Id}.{extension}";
                await using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await file.CopyToAsync(fileStream);
            }

            await _db.Lectures.AddAsync(lecture);
            await _db.SaveChangesAsync();

            if (input.ParetntLectureId != 0)
            {
                SetNextLectureId(input, lecture);
            }
        }

        private async void SetNextLectureId(CreateLecture input, Lecture lecture)
        {
            //_db.Lectures - all lectures
            var parentLecture = _db.Lectures.FirstOrDefault(x => x.Id == input.ParetntLectureId);
            parentLecture.NextLectureId = lecture.Id;
            await _db.SaveChangesAsync();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this._db.Lectures.Select(x => new
            {
                x.Id,
                x.Name
            })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }
    }
}
