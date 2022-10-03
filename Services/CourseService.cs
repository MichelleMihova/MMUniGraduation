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
                ParetntID = input.ParetntID,
               // NextCourseSignature = input,
                CourseStartDate = input.CourseStartDate,
                SkipCoursEndDate = input.SkipCoursEndDate

            };
            await _db.Courses.AddAsync(course);
            await _db.SaveChangesAsync();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this._db.Courses.Select(x => new
            {
                x.Id,
                x.Name,
            })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }
    }
}
