using SmartStart.Repository.PointOfSale.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.PointOfSale.PointOfSaleService.DataTransferObject
{
    public class CodeDetailsSimpleDto
    {
        public Guid Id { get; set; }
        public string Hash { get; set; }
        public string StudentName { get; set; }
        public double Value { get; set; }
        public DateTime? DateActivated { get; set; }
        public double DiscountRate { get; set; }
        public DateTime MaxEndDate { get; set; }
        public DateTime CreateDate { get; set; }

        public PackageDto Package { get; set; }
    }
}
