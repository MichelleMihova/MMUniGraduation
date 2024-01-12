using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MMUniGraduation.ViewModels
{
    public class EditLectorViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<IFormFile> Photos { get; set; }
    }
}
