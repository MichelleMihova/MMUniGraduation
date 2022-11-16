using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using MMUniGraduation.ViewModels;
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
        private readonly UserManager<ApplicationUser> _userManager;
        public LectureService(ApplicationDbContext db, IWebHostEnvironment webHost, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _webHost = webHost;
            _userManager = userManager;
        }
        public async Task CreateLectureAsync(CreateLecture input)
        {
            var lecture = new Lecture
            {
                Name = input.Name,
                Description = input.Description,
                CourseId = input.CourseId,
                ParetntLectureId = input.ParetntLectureId,
                VideoUrl = input.VideoUrl,
                DateTimeToShow = input.DateTimeToShow,
                EndDateTimeForHW = input.EndDateTimeForHW,
                //set currentUserId as creatorId
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

        private async Task RemoveLectureInheritance(int lectureId, Lecture currLecture)
        {
            var lectures = _db.Lectures.Where(x => x.ParetntLectureId == lectureId || x.NextLectureId == lectureId);
            foreach (var lecture in lectures)
            {
                if (lecture.NextLectureId == lectureId)
                {
                    lecture.NextLectureId = currLecture.NextLectureId;
                }
                if (lecture.ParetntLectureId == lectureId)
                {
                    lecture.ParetntLectureId = currLecture.ParetntLectureId;
                }
            }
            
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

        public async Task AddHomeworkToLecture(int lectureId, IFormFile file, string userId)
        {
            var currLectire = _db.Lectures.FirstOrDefault(x => x.Id == lectureId);

            var extension = Path.GetExtension(file.FileName).TrimStart('.');
            var wwwrootPath = _webHost.WebRootPath;

            if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
            {
                throw new Exception($"Invalid file extension {extension} !");
            }

            var homeworkFile = new Homework
            {
                Extension = extension,
                HomeworkName = file.FileName,
                StudentId = userId
            };

            currLectire.Homeworks.Add(homeworkFile);

            var physicalPath = $"{wwwrootPath}/homeworks/{homeworkFile.Id}.{extension}";
            await using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            await _db.SaveChangesAsync();
        }

        public async Task EditHomework(int homeworkId, decimal homeworkGrade, string homeworkComment)
        {
            var homework = _db.Homeworks.FirstOrDefault(h => h.Id == homeworkId);
            if (homeworkGrade > 0)
            {
                homework.Grade = homeworkGrade;
            }

            if (homeworkComment != null)
            {
                homework.Comment = homeworkComment;
            }

            await _db.SaveChangesAsync();
        }

        public async Task EditLectureFile (EditCourseViewModel input)
        {
            var lectureFile = _db.LectureFiles.FirstOrDefault(x => x.Id == input.LectureFileId);

            if (lectureFile != null && input.MinHWGrade != 0)
            {
                lectureFile.MinHWGrade = input.MinHWGrade;
            }

            if (lectureFile != null && input.DateTimeToShowFile != lectureFile.DateTimeToShow)
            {
                lectureFile.DateTimeToShow = input.DateTimeToShowFile;
            }

            await _db.SaveChangesAsync();
        }

        public async Task EditLecture(EditCourseViewModel input)
        {
            var lecture = _db.Lectures.FirstOrDefault(l => l.Id == input.LectureId);

            if (input.LectureDescription != null)
            {
                lecture.Description = input.LectureDescription;
            }

            if (input.VideoUrl != null && lecture.VideoUrl != input.VideoUrl)
            {
                lecture.VideoUrl = input.VideoUrl;
            }

            if (lecture != null && input.DateTimeToShow != lecture.DateTimeToShow)
            {
                lecture.DateTimeToShow = input.DateTimeToShow;
            }

            if (input.Files != null)
            {
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
            }

            await _db.SaveChangesAsync();
        }

        public async Task DeleteLectureMaterial(string lectureFileId)
        {
            var lectureFile = _db.LectureFiles.FirstOrDefault(l => l.Id == lectureFileId);

            _db.LectureFiles.Remove(lectureFile);

            await _db.SaveChangesAsync();
        }

        public async Task DeleteLectureMaterial(int lectureId)
        {
            var lectureFiles = _db.LectureFiles.Where(l => l.LectureId == lectureId).ToArray();

            foreach (var file in lectureFiles)
            {
                _db.LectureFiles.Remove(file);
            }

            await _db.SaveChangesAsync();
        }

        public async Task DeleteHomework(int lectureId)
        {
            var homeworks = _db.Homeworks.Where(l => l.LectureId == lectureId).ToArray();

            foreach (var homework in homeworks)
            {
                _db.Homeworks.Remove(homework);
            }

            await _db.SaveChangesAsync();
        }

        public async Task DeleteLecture(int lectureId)
        {
            var lecture = _db.Lectures.FirstOrDefault(l => l.Id == lectureId);

            await RemoveLectureInheritance(lectureId, lecture);

            _db.Lectures.Remove(lecture);
            await _db.SaveChangesAsync();
        }

    }
}
