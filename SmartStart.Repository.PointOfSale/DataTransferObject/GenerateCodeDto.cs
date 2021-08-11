using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.PointOfSale.PointOfSaleService.DataTransferObject
{
    public class GenerateCodeDto
    {
       public float DiscountRate { get; set; }
       public IEnumerable<Guid> PackageIds { get; set; }
    }
}
