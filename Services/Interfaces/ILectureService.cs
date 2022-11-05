using Microsoft.AspNetCore.Http;
using MMUniGraduation.Models.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Services.Interfaces
{
    public interface ILectureService
    {
        //public Task CreateAsync(string name, string description, string paretntLectureSignature, string nextLectureSignature, DateTime dateTimeToShow, DateTime endDateTimeForHW);
        public Task CreateLectureAsync(CreateLecture input);
        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
        public Task AddHomeworkToLecture(int lectureId, IFormFile file, string userId);
        public Task DeleteLecture(int courseId);
        public Task EditHomework(int homeworkId, decimal homeworkGrade, string homeworkComment);
        public Task EditLecture(int lectureId, string lectureDescription, int courseId);
    }
}
