using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.InvoiceDto
{
    public class InvoiceDto
    {
        public Guid InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public string InvoiceNumber { get; set; }
        public int CodeCount { get; set; }
        public double Total { get; set; }
    }
}
