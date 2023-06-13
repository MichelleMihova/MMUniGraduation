using MMUniGraduation.Models;
using MMUniGraduation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Services.Interfaces
{
    public interface IStudentService
    {
        //public Task AddStudentAsync(Student input);
        public Task EditStudent(EditStudentViewModel input);
    }
}
