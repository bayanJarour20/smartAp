using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.CodeDto
{
    public class CodeDescriptionDto
    {
        public double Value { get; set; }
        public string Hash { get; set; }
        public double DiscountRate { get; set; }
        public DateTime MaxEndDate { get; set; }
        public CodeTypes Type { get; set; }
        public string Description { get; set; }
    }
}
