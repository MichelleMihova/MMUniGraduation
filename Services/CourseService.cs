using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Services
{
    public class CourseService: ICourseService
    {
        private readonly ApplicationDbContext _db;
        public CourseService(ApplicationDbContext db)
        {
            _db = db;
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
                CreatorId = user.Id
            };
            await _db.Courses.AddAsync(course);
            await _db.SaveChangesAsync();

            if (input.ParetntId != 0)
            {
                SetNextCourseId(input, course);
            }
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

        /*
        //public string GetNextCourseSuggestion(ApplicationUser user)
        public string GetNextCourseSuggestion(Student user)
        {
            string Name;
            if (user != null && user.CurrentCourseId != null)
            {
                if (_db.Courses.FirstOrDefault(x => x.Id == user.CurrentCourseId).NextCourseId != 0)
                {
                    Name = _db.Courses.FirstOrDefault(x => x.ParetntId == user.CurrentCourseId).Name.ToString();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (user != null && user.CurrentCourseId == null)
                {
                    var passedCourses = _db.StudentCourses.Where(x => x.StudentId == user.Id);
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
                            Name = _db.Courses.FirstOrDefault(x => x.ParetntId == maxNum).Name.ToString();
                        }
                        else
                        {
                            Name = "Congrats! You compleate the program!";
                        }
                    }
                    else
                    {
                        Name = _db.Courses.FirstOrDefault(x => x.ParetntId == 0).Name.ToString();
                        //Name = "Basic";
                    }
                }
                else
                {
                    Name = _db.Courses.FirstOrDefault(x => x.ParetntId == 0).Name.ToString();
                    //Name = "Basic";
                }
                //Name = _db.Courses.FirstOrDefault().Name;
                //Name = "Basic";
            }

            return Name;
        }
        */

        public string GetNextCourseSuggestion(Student user, int programId)
        {
            string Name = null;

            var currCourseId = 0;
            
            //var currCourseId = _db.StudentCourses.FirstOrDefault(x => x.StudentId == user.Id && x.IsPassed == false && x.ProgramId == programId).CourseId;
           
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
                            Name = _db.Courses.FirstOrDefault(x => x.ParetntId == maxNum).Name.ToString();
                        }
                        else
                        {
                            Name = "Congrats! You compleate the program!";
                        }
                    }
                    else
                    {
                        Name = _db.Courses.FirstOrDefault(x => x.ParetntId == 0).Name.ToString();
                    }
                }
                else
                {
                    //if we have curr course
                    if (_db.Courses.FirstOrDefault(x => x.Id == currCourseId).NextCourseId != 0)
                    {
                        Name = _db.Courses.FirstOrDefault(x => x.ParetntId == currCourseId).Name.ToString();
                    }
                }
                
            }
            else
            {
                Name = _db.Courses.FirstOrDefault(x => x.ParetntId == 0).Name.ToString();
            }
            return Name;
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
