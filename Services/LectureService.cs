using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Services
{
    public class LectureService : ILectureService
    {
        private readonly ApplicationDbContext _db;
        public LectureService(ApplicationDbContext db)
        {
            _db = db;
        }
        //public async Task CreateAsync(string name, string description, string paretntLectureSignature, string nextLectureSignature, DateTime dateTimeToShow, DateTime endDateTimeForHW)
        public async Task CreateLectureAsync(CreateLecture input)
        {
            var lecture = new Lecture
            {
                Name = input.Name,
                Description = input.Description,
                CourseId = input.CourseId,
                ParetntLectureId = input.ParetntLectureId,
                //NextLectureSignature = nextLectureSignature,
                DateTimeToShow = input.DateTimeToShow,
                EndDateTimeForHW = input.EndDateTimeForHW

            };
            await _db.Lectures.AddAsync(lecture);
            await _db.SaveChangesAsync();

            if (input.ParetntLectureId != 0)
            {
                SetNextLectureId(input, lecture);
            }
        }

        private async void SetNextLectureId(CreateLecture input, Lecture lecture)
        {
            //_db.Lectures - all lectures
            var parentLecture = _db.Lectures.FirstOrDefault(x => x.Id == input.ParetntLectureId);
            parentLecture.NextLectureId = lecture.Id;
            await _db.SaveChangesAsync();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this._db.Lectures.Select(x => new
            {
                x.Id,
                x.Name
            })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));

        }
       
    }
}
