using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using SmartStart.Model.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartStart.Model.Business
{
    public class PackageSubjectFaculty : BaseEntity<Guid>
    {
        /// <summary>
        /// Entered price when connected with Package
        /// </summary>
        [ColumnDataType(DataBaseTypes.FLOAT)]
        public double Price { get; set; }

        [Required]
        public Guid PackageId { get; set; }
        public Package Package { get; set; }

        [Required]
        public Guid SubjectFacultyId { get; set; }
        public SubjectFaculty SubjectFaculty { get; set; }
    }
}
