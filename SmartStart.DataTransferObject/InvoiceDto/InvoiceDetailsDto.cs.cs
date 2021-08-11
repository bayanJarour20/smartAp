using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.InvoiceDto
{
    public class InvoiceDetailsDto
    {
        public string Name { get; set; }
        public IEnumerable<InvoiceDto> Invoices { get; set; }
    }
}
