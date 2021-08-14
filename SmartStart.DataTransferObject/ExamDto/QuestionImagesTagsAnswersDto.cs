using SmartStart.DataTransferObject.QuestionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.ExamDto
{
    public class QuestionImagesTagsAnswersDto : QuestionTagsAnswersDto
    {
        public IEnumerable<QuestionDocumentNote> Images { get; set; }
    }
}
