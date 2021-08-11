using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.CodeDto
{
    public class CodeGenerateDto
    {
        public Guid Id { get; set; }
        public float DiscountRate { get; set; }
        public Guid SellerId { get; set; }
        public IEnumerable<Guid> PackageIds { get; set; }
    }
}
