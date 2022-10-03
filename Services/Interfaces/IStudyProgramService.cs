using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMUniGraduation.Services.Interfaces
{
    public interface IStudyProgramService
    {
        public Task CreateAsync(string name, string description);
        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}
