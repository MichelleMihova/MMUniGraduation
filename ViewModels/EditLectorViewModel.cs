using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.ViewModels
{
    public class EditLectorViewModel
    {
        public int Id { get; set; }
        //public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<IFormFile> Photos { get; set; }
    }
}
