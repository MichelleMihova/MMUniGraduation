﻿using MMUniGraduation.ViewModels;
using System.Threading.Tasks;

namespace MMUniGraduation.Services.Interfaces
{
    public interface IStudentService
    {
        public Task EditStudent(EditStudentViewModel input);
    }
}
