using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.CodeDto
{
    public class CodeSubjectsPriceDto : CodeDto
    {
        public IEnumerable<BaseSubjectPriceDto> Subjects { get; set; }
    }
    public class BaseSubjectPriceDto
    {
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; }
        public double Price { get; set; }
    }
}
