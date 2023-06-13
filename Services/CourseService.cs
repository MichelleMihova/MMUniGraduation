using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class CourseService: ICourseService
    {
        private readonly string[] allowedExtensions = new[] { "doc", "docx", "txt", "pptx", "pptm", "pdf" };

        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHost;
        private readonly ILectureService _lectureService;

        public CourseService(ApplicationDbContext db, IWebHostEnvironment webHost, ILectureService lectureService)
        {
            _db = db;
            _webHost = webHost;
            _lectureService = lectureService;
        }

        public async Task CreateCourseAsync(CreateCourse input, ApplicationUser user)
        {
            var course = new Course
            {
                Name = input.Name,
                Signature = _db.StudyPrograms.FirstOrDefault(x => x.Id == input.StudyProgramId).Name,
                Description = input.Description,
                StudyProgramId = input.StudyProgramId,
                ParetntId = input.ParetntId,
                CourseStartDate = input.CourseStartDate,
                SkipCoursEndDate = input.SkipCoursEndDate,
                CreatorId = user.Id,
                RequiredSkippingCourseGrade = input.RequiredSkippingCourseGrade
            };

            await _lectureService.CreateLectureFile(null, input.SkippingCourseFiles, "SKIPPINGEXAM", course);
            await _db.Courses.AddAsync(course);
            await _db.SaveChangesAsync();

            if (input.ParetntId != 0)
            {
                SetNextCourseId(input, course);
            }
        }
        public async Task AddSkippingExamSolutionToCourse(int courseId, IFormFile file, string userId)
        {
            var currCourse = _db.Courses.FirstOrDefault(x => x.Id == courseId);

            var extension = Path.GetExtension(file.FileName).TrimStart('.');
            var wwwrootPath = _webHost.WebRootPath;

            if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
            {
                throw new Exception($"Invalid file extension {extension} !");
            }

            var skippingAssignmentFile = new SkippingAssignment
            {
                Extension = extension,
                Name = file.FileName,
                StudentId = userId,
                Title = "SkippingExam"
            };

            currCourse.SkippingAssignments.Add(skippingAssignmentFile);

            var physicalPath = $"{wwwrootPath}/skippingExamSolutions/{skippingAssignmentFile.Id}.{extension}";
            await using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            await _db.SaveChangesAsync();
        }
        private async void SetNextCourseId(CreateCourse input, Course course)
        {
            var parentCourse = _db.Courses.FirstOrDefault(x => x.Id == input.ParetntId);
            parentCourse.NextCourseId = course.Id;
            await _db.SaveChangesAsync();
        }
        private async Task RemoveCourseInheritance(int courseId, Course currCourse)
        {
            var courses = _db.Courses.Where(x => x.ParetntId == courseId || x.NextCourseId == courseId);
            foreach (var course in courses)
            {
                if (course.NextCourseId == courseId)
                {
                    course.NextCourseId = currCourse.NextCourseId;
                }
                if (course.ParetntId == courseId)
                {
                    course.ParetntId = currCourse.ParetntId;
                }
            }

            await _db.SaveChangesAsync();
        }
        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this._db.Courses.Select(x => new
            {
                x.Id,
                x.Name,
                x.Signature
            })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Signature + " "+ x.Name));
        }
        public string GetNextCourseSuggestion(Student user, int programId)
        {
            string Name = null;
            string suggestionText = "The more appropriate next course for you is ";
            var currCourseId = 0;
            
            //if we have user and some curr courses for the same program
            if (user != null)
            {
                if (_db.StudentCourses.FirstOrDefault(x => x.StudentId == user.Id && x.IsPassed == false && x.ProgramId == programId) != null)
                {
                    currCourseId = _db.StudentCourses.FirstOrDefault(x => x.StudentId == user.Id && x.IsPassed == false && x.ProgramId == programId).CourseId;
                }

                if (currCourseId == 0)
                {
                    var passedCourses = _db.StudentCourses.Where(x => x.StudentId == user.Id && x.IsPassed == true && x.ProgramId == programId);

                    if (passedCourses.Any())
                    {
                        //make it smarter!!!
                        int num = 0;
                        int maxNum = 0;
                        foreach (var item in passedCourses)
                        {
                            num = item.CourseId;
                            if (num > maxNum)
                            {
                                maxNum = num;
                                num = 0;
                            }
                        }

                        if (_db.Courses.FirstOrDefault(x => x.Id == maxNum).NextCourseId != 0)
                        {
                            Name = suggestionText + _db.Courses.FirstOrDefault(x => x.ParetntId == maxNum).Name.ToString();
                        }
                        else
                        {
                            Name = "Congrats! You compleate the program!";
                        }
                    }
                    else
                    {
                        if (_db.Courses.Any())
                        {
                            Name = suggestionText + _db.Courses.FirstOrDefault(x => x.ParetntId == 0).Name.ToString();
                        }
                    }
                }
                else
                {
                    //if we have curr course
                    if (_db.Courses.FirstOrDefault(x => x.Id == currCourseId).NextCourseId != 0)
                    {
                        Name = suggestionText + _db.Courses.FirstOrDefault(x => x.ParetntId == currCourseId).Name.ToString();
                    }
                    else
                    {
                        Name = "Congrats! You compleate the program!";
                    }
                }
                
            }
            else
            {
                if (_db.Courses.Any())
                {
                    Name = suggestionText + _db.Courses.FirstOrDefault(x => x.ParetntId == 0).Name.ToString();
                }
                //Name = suggestionText + _db.Courses.FirstOrDefault(x => x.ParetntId == 0).Name.ToString();
            }
            return Name;
        }
        public async Task DeleteSkippingCourseMaterial(int courseId)
        {
            var lectureFiles = _db.LectureFiles.Where(x => x.CourseId == courseId).ToArray();

            foreach (var file in lectureFiles)
            {
                _db.LectureFiles.Remove(file);
            }

            await _db.SaveChangesAsync();
        }
        public async Task DeleteSkippingAssignment(int courseId)
        {
            var skippingAssignments = _db.SkippingAssignments.Where(x => x.CourseId == courseId).ToArray();

            foreach (var skippingAssignment in skippingAssignments)
            {
                _db.SkippingAssignments.Remove(skippingAssignment);
            }

            await _db.SaveChangesAsync();
        }
        public async Task DeleteCourse(int courseId)
        {
            var course = _db.Courses.FirstOrDefault(c => c.Id == courseId);
            await RemoveCourseInheritance(courseId, course);

            _db.Courses.Remove(course);

            await _db.SaveChangesAsync();
        }

    }
}
