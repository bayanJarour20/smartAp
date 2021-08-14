using SmartStart.DataTransferObject.ExamDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.QuestionDto
{
    public class QuestionTagsAnswersExamsDto : QuestionImagesTagsAnswersDto
    {
        public IEnumerable<QuestionExamDto> Exams { get; set; }
    }
}
