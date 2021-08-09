using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.InvoiceDto
{
    public class CreateInvoiceDto
    {
        public Guid InvoiceId { get; set; }
        public Guid PosId { get; set; }
        public string PosName { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public int CodeCount { get; set; }
        public double ActualCost { get; set; }
        public double DueCompany { get; set; }
        public double PaidMoney { get; set; }
        public string Notes { get; set; }
        public List<CodeInvoiceDto> Codes { get; set; }
    }
}
