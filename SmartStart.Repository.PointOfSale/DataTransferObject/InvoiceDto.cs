using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.PointOfSale.PointOfSaleService.DataTransferObject
{
    public class InvoiceDto
    {
        public string SerialNumber { get; set; }
        public DateTime Date { get; set; }
        public float Rate { get; set; }
        public double Total { get; set; }
        public string Note { get; set; }
    }
}
