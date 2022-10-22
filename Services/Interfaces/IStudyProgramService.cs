using MMUniGraduation.Models.Create;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMUniGraduation.Services.Interfaces
{
    public interface IStudyProgramService
    {
        public Task CreateAsync(CreateStudyProgram model);
        IEnumerable<T> GetPrograms<T>(int count);
        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}
