using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.QuestionDto
{
    public class QuestionAnswersDto : QuestionDto
    {
        public IEnumerable<AnswerDto> Answers { get; set; }
    }
}
