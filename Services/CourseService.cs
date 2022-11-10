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

        public async Task CreateCourseAsync(CreateCourse input)
        {
            var course = new Course
            {
                Name = input.Name,
                Signature = _db.StudyPrograms.FirstOrDefault(x => x.Id == input.StudyProgramId).Name,
                Description = input.Description,
                StudyProgramId = input.StudyProgramId,
                ParetntId = input.ParetntId,
                CourseStartDate = input.CourseStartDate,
                SkipCoursEndDate = input.SkipCoursEndDate

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

        public string GetNextCourseSuggestion(ApplicationUser user)
        {
            string Name;
            if (user.CurrentCourseId != 0)
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
                //Name = _db.Courses.FirstOrDefault().Name;
                Name = "Basic";
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
