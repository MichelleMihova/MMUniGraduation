using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMUniGraduation.Services.Interfaces
{
    public interface ICourseService
    {
        public Task CreateCourseAsync(CreateCourse input, ApplicationUser user);
        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
        public string GetNextCourseSuggestion(Student user);
        public Task DeleteCourse(int courseId);
    }
}
