using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.PackageDto
{
    public class PackageSubjectDto : PackageDto
    {
        public IEnumerable<PriceSubjectDto> Subjects { get; set; }
    }
}
