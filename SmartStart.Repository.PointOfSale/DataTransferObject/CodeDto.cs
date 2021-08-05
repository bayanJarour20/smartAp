using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.PointOfSale.PointOfSaleService.DataTransferObject
{
    public class CodeDto
    {
        public string Hash { get; set; }

        public double Value { get; set; }

        public double DiscountRate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateActivated { get; set; }
        public DateTime MaxEndDate { get; set; }
        public CodeTypes Type { get; set; }
        public bool IsInvoice { get; set; }

    }
}
