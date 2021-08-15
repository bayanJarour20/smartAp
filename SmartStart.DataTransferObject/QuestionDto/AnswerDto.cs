using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.QuestionDto
{
    public class AnswerDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime? CorrectionDate { get; set; }
        public Guid QuestionId { get; set; }
        public Guid? CorrectAnswerId { get; set; }
    }
}
