using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.PackageDto
{
    public class PackageSubjectFilterDto : PackageSubjectDto
    {
        public object Filter { get; set; }
    }
}
