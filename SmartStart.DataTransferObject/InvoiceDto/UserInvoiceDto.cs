using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.InvoiceDto
{
    public class UserInvoiceDto
    {
        public Guid PosId { get; set; }
        public string PosName { get; set; }
        public int CodeCount { get; set; }
        public DateTime? LastMatchDate { get; set; }
        public double PaidMoney { get; set; }
        public double BalanceDue { get; set; }
    }
}
