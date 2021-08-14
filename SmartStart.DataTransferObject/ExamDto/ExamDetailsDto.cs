using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.ExamDto
{
    public class ExamDetailsDto : ExamDto
    {
        public DateTime DateCreated { get; set; }
        //public SubjectDto Subject { get; set; }
        public Guid SemesterId { get; set; }
        public int QuestionsCount { get; set; }
    }
}
