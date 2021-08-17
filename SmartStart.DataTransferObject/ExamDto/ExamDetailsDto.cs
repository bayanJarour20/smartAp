using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartStart.DataTransferObject.SubjectDto;

namespace SmartStart.DataTransferObject.ExamDto
{
    public class ExamDetailsDto : ExamDto
    {
        public DateTime DateCreated { get; set; }
        public string SubjectName { get; set; }
        public Guid SemesterId { get; set; }
        public Guid SectionId { get; set; }
        public int QuestionsCount { get; set; }
    }
}
