using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.ExamDto
{
    public class BaseCollectionExamDto
    {
        public IEnumerable<BaseExamDto> Exams { get; set; }
        public IEnumerable<BaseExamDto> Banks { get; set; }
    }
}
