using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.ExamDto
{
    public class ExamDocumentDto
    {
        public Guid Id { get; set; }
        public string Note { get; set; }
        public string Path { get; set; }
        public IFormFile File { get; set; }
    }
}
