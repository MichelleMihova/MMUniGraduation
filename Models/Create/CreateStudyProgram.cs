using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MMUniGraduation.Models.Create
{
    public class CreateStudyProgram
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<IFormFile> Images { get; set; }
    }
}
