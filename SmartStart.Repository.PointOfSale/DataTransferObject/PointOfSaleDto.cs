using SmartStart.Repository.PointOfSale.PointOfSaleService.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.PointOfSale.DataTransferObject
{
    public class PointOfSaleDto
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool AllowDiscount { get; set; }
        public string Address { get; set; }
        public double? MoneyLimit { get; set; }
        public double Rate { get; set; }
        public Guid CityId { get; set; }
        public IEnumerable<CodePackagesDto> Codes { get; set; }
        public IEnumerable<InvoiceDto> Invoices { get; set; }

    }
}
