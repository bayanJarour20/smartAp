using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.FacultyDto
{
    public class FacultyDto : FacultyBaseDto
    {
        public string ImagePath { get; set; }
        public Guid UniversityId { get; set; }
        public DateTime DateCreate { get; set; }
        public IFormFile File { get; set; }
    }
}
