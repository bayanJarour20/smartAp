using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.QuestionDto
{
    public class QuestionExamDto
    {
        public Guid ExamId { get; set; }
        public Guid QuestionId { get; set; }
        public short Order { get; set; }
        public string ExamName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
