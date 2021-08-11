using SmartStart.Repository.PointOfSale.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.PointOfSale.PointOfSaleService.DataTransferObject
{
    public class CodePackagesDto : CodeDto
    {
        public IEnumerable<PackageDto> Packages { get; set; }
    }
}
