using MMUniGraduation.Models.Create;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMUniGraduation.Services.Interfaces
{
    public interface ICourseService
    {
        public Task CreateCourseAsync(CreateCourse input);
        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}
