using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MMUniGraduation.ViewModels
{
    public class EditStudentViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool ShowTextMaterials { get; set; }
        public bool ShowVideoMaterials { get; set; }
        public IEnumerable<IFormFile> Photos { get; set; }
    }
}
