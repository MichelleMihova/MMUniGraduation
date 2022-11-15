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

        public async Task AddStudentAsync(Student input)
        {
            var student = new Student
            {
               

            };
            //await _db.Courses.AddAsync(course);
            await _db.SaveChangesAsync();
           
        }

    }
}
