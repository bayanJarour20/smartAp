using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using SmartStart.Model.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartStart.Model.Main
{
    public class SubjectFacultyAppUser : BaseEntity<Guid>
    {
        [ColumnDataType(DataBaseTypes.BIT)]
        public bool DefaultSelected { get; set; }

        [Required]
        public Guid SubjectFacultyId { get; set; }
        public SubjectFaculty Subject { get; set; }

        [Required]
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
