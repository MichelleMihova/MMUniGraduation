using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMUniGraduation.Models
{
    public class LearningObject
    {
        public int ID { get; set; }
        public string VideoMaterial { get; set; }

        [NotMapped]
        public IEnumerable<IFormFile> TextMaterial { get; set; }
    }
}
