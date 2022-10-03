using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Services
{
    public class StudyProgramService: IStudyProgramService
    {
        private readonly ApplicationDbContext _db;
        public StudyProgramService(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this._db.StudyPrograms.Select(x => new
            {
                x.Id,
                x.Name,
            })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }
        public async Task CreateAsync(string name, string description)
        {
            var studyProgram = new StudyProgram
            {
                Name = name,
                Description = description,

            };
            await _db.StudyPrograms.AddAsync(studyProgram);
            await _db.SaveChangesAsync();
        }
    }
}
