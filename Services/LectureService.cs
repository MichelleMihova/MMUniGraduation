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
        public async Task CreateLectureFile(Lecture lecture, IEnumerable<IFormFile> files, string type, Course course)
        {
            foreach (var file in files)
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
                    FileName = file.FileName,
                    FileTitle = type
                };

                if (lecture != null)
                {
                    lecture.TextMaterials.Add(lectureFile);
                }

                if (course != null)
                {
                    course.SkippingCourseMaterials.Add(lectureFile);
                }

                var physicalPath = $"{wwwrootPath}/files/{lectureFile.Id}.{extension}";
                await using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await file.CopyToAsync(fileStream);
            }

            await _db.SaveChangesAsync();
        }
        public async Task IsPassed(Lecture lecture, string studentId)
        {
            var courseId = lecture.CourseId;

            //get all exam lectures lectures
            var assessmentLecturesId = _db.Lectures.Where(x => x.RequiredGrade != 0 && x.CourseId == courseId && x.IsExam == true).Select(x => x.Id);
            var student = _db.Students.FirstOrDefault(x => x.UserId == studentId);
            
            bool passed = true;
            decimal finalGrade = 0;
            var cnt = 0;

            foreach (var assessmentLectureId in assessmentLecturesId)
            {
                var homeworkGrades = _db.Homeworks.Where(x => x.LectureId == assessmentLectureId && x.StudentId == student.UserId).Select(x => x.Grade).ToList();
                decimal avGrade = 0;
                foreach (var grade in homeworkGrades)
                {
                    avGrade += grade;
                }
                avGrade /= homeworkGrades.Count();
                finalGrade += avGrade;
                cnt++;

                if (avGrade < _db.Lectures.FirstOrDefault(x => x.Id == assessmentLectureId).RequiredGrade)
                {
                    passed = false;
                }
            }

            if (passed)
            {
                //mark course as passed
                var studentCourse = _db.StudentCourses.FirstOrDefault(x => x.CourseId == courseId && !x.IsPassed && x.StudentId == student.Id);
                finalGrade /= cnt;

                studentCourse.IsPassed = true;
                studentCourse.FinalGrade = finalGrade;

                //student.CurrentCourseId = null;

                await _db.SaveChangesAsync();
            }

        }
        public async Task CreateLectureAsync(CreateLecture input, ApplicationUser user)
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
                CreatorId = user.Id,
                IsFinal = input.IsFinal,
                IsExam = input.IsExam,
                RequiredGrade = input.RequiredGrade
            };

            var haveFinalLecture =  _db.Lectures.FirstOrDefault(x => x.CourseId == input.CourseId && x.IsFinal == true);
            if (haveFinalLecture != null)
            {
                lecture.IsFinal = false;
                lecture.IsExam = false;
            }

            await CreateLectureFile(lecture, input.Files, "LECTURE", null);
            await CreateLectureFile(lecture, input.HWFiles, "HOMEWORK", null);

            await _db.Lectures.AddAsync(lecture);
            await _db.SaveChangesAsync();

            if (input.ParetntLectureId != 0)
            {
                SetNextLectureId(input, lecture);
            }

            //Check all lectures that are enought to finish the course when lecture is final
            if (input.IsFinal)
            {
                var courseId = lecture.CourseId;
                var studentsId = _db.StudentCourses.Where(x => x.CourseId == courseId && !x.IsPassed).Select(x => x.StudentId).ToList();

                foreach (var id in studentsId)
                {
                    var studentId = _db.Students.FirstOrDefault(x => x.Id == id).UserId;
                    await IsPassed(lecture, studentId);
                }
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

        public async Task AddExamSolutionToLecture(int lectureId, IFormFile file, string userId)
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
                StudentId = userId,
                HomeworkTitle = "Exam"
            };

            currLectire.Homeworks.Add(homeworkFile);

            var physicalPath = $"{wwwrootPath}/examSolutions/{homeworkFile.Id}.{extension}";
            await using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            await _db.SaveChangesAsync();
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
                StudentId = userId,
                HomeworkTitle = "Homework"
            };

            currLectire.Homeworks.Add(homeworkFile);

            var physicalPath = $"{wwwrootPath}/homeworks/{homeworkFile.Id}.{extension}";
            await using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            await _db.SaveChangesAsync();
        }

        public async Task EditHomework(string homeworkId, decimal homeworkGrade, string homeworkComment)
        {
            var homework = _db.Homeworks.FirstOrDefault(x => x.Id == homeworkId);
            if (homeworkGrade > 0)
            {
                homework.Grade = homeworkGrade;
            }

            if (homeworkComment != null)
            {
                homework.Comment = homeworkComment;
            }

            await _db.SaveChangesAsync();

            var lecture = _db.Lectures.FirstOrDefault(x => x.Id == homework.LectureId);
            var hasFinalLecture = _db.Lectures.FirstOrDefault(x => x.CourseId == lecture.CourseId && x.IsFinal == true);
            //check first if we have final lectire for this course
            if (hasFinalLecture != null && homework.HomeworkTitle.ToUpper() != "EXAM")
            {
                await IsPassed(lecture, homework.StudentId);
            }
        }
        public async Task EditSkippingAssignment(string skippingAssignmentId, decimal grade, string comment)
        {
            var skippingAssignment = _db.SkippingAssignments.FirstOrDefault(x => x.Id == skippingAssignmentId);
            var course = _db.Courses.FirstOrDefault(x => x.Id == skippingAssignment.CourseId);
            var student = _db.Students.FirstOrDefault(x => x.UserId == skippingAssignment.StudentId);

            if (grade > 0)
            {
                skippingAssignment.Grade = grade;
            }

            if (comment != null)
            {
                skippingAssignment.Comment = comment;
            }

            await _db.SaveChangesAsync();

            if (grade >= course.RequiredSkippingCourseGrade)
            {
                //mark course as passed
                var studentCourse = _db.StudentCourses.FirstOrDefault(x => x.CourseId == course.Id && !x.IsPassed && x.StudentId == student.Id);
                //finalGrade /= cnt;

                studentCourse.IsPassed = true;
                studentCourse.FinalGrade = grade;

                await _db.SaveChangesAsync();
            }
        }

        public async Task EditLectureFile(EditCourseViewModel input)
        {
            var lectureFile = _db.LectureFiles.FirstOrDefault(x => x.Id == input.LectureFileId);

            if (lectureFile != null && input.MinHWGrade != 0)
            {
                lectureFile.MinHWGrade = input.MinHWGrade;
            }

            if (lectureFile != null && input.DateTimeToShowFile != lectureFile.DateTimeToShow && input.DateTimeToShowFile != Convert.ToDateTime("1.1.0001 г. 0:00:00"))
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

            if (lecture != null && input.DateTimeToShow != lecture.DateTimeToShow && input.DateTimeToShow != Convert.ToDateTime("1.1.0001 г. 0:00:00"))
            {
                lecture.DateTimeToShow = input.DateTimeToShow;
            }

            if (input.Files != null)
                await CreateLectureFile(lecture, input.Files, "LECTURE", null);

            if (input.HWFiles != null)
                await CreateLectureFile(lecture, input.HWFiles, "HOMEWORK", null);

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
