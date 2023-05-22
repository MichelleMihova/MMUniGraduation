using Microsoft.AspNetCore.Http;
using MMUniGraduation.Models;
using MMUniGraduation.Models.Create;
using MMUniGraduation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Services.Interfaces
{
    public interface ILectureService
    {
        //public Task CreateAsync(string name, string description, string paretntLectureSignature, string nextLectureSignature, DateTime dateTimeToShow, DateTime endDateTimeForHW);
        public Task CreateLectureFile(Lecture lecture, IEnumerable<IFormFile> files, string type, Course course);
        public Task IsPassed(Lecture lecture, string studentId);
        public Task CreateLectureAsync(CreateLecture input, ApplicationUser user);
        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
        public Task AddExamSolutionToLecture(int lectureId, IFormFile file, string userId);
        public Task AddHomeworkToLecture(int lectureId, IFormFile file, string userId);
        public Task EditHomework(string homeworkId, decimal homeworkGrade, string homeworkComment);
        public Task EditSkippingAssignment(string skippingAssignmentId, decimal grade, string comment);
        public Task EditLectureFile(EditCourseViewModel input);
        public Task EditLecture(EditCourseViewModel input);
        public Task DeleteLecture(int lectureId);
        public Task DeleteLectureMaterial(string lectureFileId);
        public Task DeleteLectureMaterial(int lectureId);
        public Task DeleteHomework(int lectureId);
    }
}
