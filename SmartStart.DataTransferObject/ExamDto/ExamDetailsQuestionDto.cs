using SmartStart.DataTransferObject.QuestionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.ExamDto
{
    public class ExamDetailsQuestionDto : ExamDetailsDto
    {
        public IEnumerable<QuestionTagsAnswersDto> Questions { get; set; }
        public IEnumerable<ExamDocumentDto> Documents { get; set; }
    }
}
