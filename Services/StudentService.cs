using MMUniGraduation.Data;
using MMUniGraduation.Models;
using MMUniGraduation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Services
{
    public class StudentService: IStudentService
    {
        private readonly ApplicationDbContext _db;
        public StudentService(ApplicationDbContext db)
        {
            _db = db;
        }

        //public async Task AddStudentAsync(Student input)
        //{
        //    var student = new Student
        //    {
               

        //    };
        //    //await _db.Courses.AddAsync(course);
        //    await _db.SaveChangesAsync();
           
        //}
        public async Task EditStudent(Student input)
        {
            var currStudent = _db.Students.FirstOrDefault(x => x.Id == input.Id);
            if (input.FirstName != null && input.FirstName != currStudent.FirstName)
            {
                currStudent.FirstName = input.FirstName;
            }

            if (input.LastName != null && input.LastName != currStudent.LastName)
            {
                currStudent.LastName = input.LastName;
            }

            if (input.ShowTextMaterials != currStudent.ShowTextMaterials)
            {
                currStudent.ShowTextMaterials = input.ShowTextMaterials;
            }

            if (input.ShowVideoMaterials != currStudent.ShowVideoMaterials)
            {
                currStudent.ShowVideoMaterials = input.ShowVideoMaterials;
            }

            await _db.SaveChangesAsync();
        }

    }
}
