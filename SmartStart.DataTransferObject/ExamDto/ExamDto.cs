using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.ExamDto
{
    public class ExamDto : ExamBaseDto
    {
        public double Price { get; set; }
        public bool IsFree { get; set; }
        public IEnumerable<Guid> TagIds { get; set; }
    }
}
