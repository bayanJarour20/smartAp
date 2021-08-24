using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.PointOfSale.PointOfSaleService.DataTransferObject
{
    public class PointOfSaleAccountCodesDto : PointOfSaleAccountDto
    {
        public IEnumerable<CodeDetailsSimpleDto> CodeDetailsSimpleDto { get; set; }
    }
}
