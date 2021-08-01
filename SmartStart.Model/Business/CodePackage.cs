using Elkood.Web.Infrastructure.ModelEntity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartStart.Model.Business
{
    public class CodePackage : BaseEntity<Guid>
    {
        [Required]
        public Guid CodeId { get; set; }
        public Code Code { get; set; }

        [Required]
        public Guid PackageId { get; set; }
        public Package Package { get; set; }
    }
}
