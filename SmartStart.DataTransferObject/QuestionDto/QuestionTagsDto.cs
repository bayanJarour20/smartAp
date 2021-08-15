using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.QuestionDto
{
    public class QuestionTagsDto : QuestionDto
    {
        /// <summary>
        /// ExamQuestion Order
        /// </summary>
        public short Order { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
    }
}
