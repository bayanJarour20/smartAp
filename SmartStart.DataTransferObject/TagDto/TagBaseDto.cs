using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.TagDto
{
    public class TagBaseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TagTypes Type { get; set; }
    }
}
