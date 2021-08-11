using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.InvoiceDto
{
    public class CodeInvoiceDto
    {
        public Guid CodeId { get; set; }
        public string Code { get; set; }
        public DateTime? Date { get; set; }
        public double DiscountRate { get; set; }
        public double PosRate { get; set; }
        public double Value { get; set; }
    }
}
