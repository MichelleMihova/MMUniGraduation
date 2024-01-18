using MMUniGraduation.Models.Create;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMUniGraduation.Services.Interfaces
{
    public interface IStudyProgramService
    {
        public Task CreateAsync(CreateStudyProgram model);
        IEnumerable<T> GetRandomPrograms<T>(int count);
        IEnumerable<T> GetAllPrograms<T>();
        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
        public Task DeleteProgram(int programId);
    }
}
