﻿using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using SmartStart.Model.General;
using SmartStart.Model.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Model.Main
{
    public class SubjectFaculty : BaseEntity<Guid>
    {
        [Required]
        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }

        [Required]
        public Guid FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        [ColumnDataType(DataBaseTypes.SMALLINT)]
        public int Year { get; set; }

        [Required]
        public Guid SemesterId { get; set; }
        public Tag Semester { get; set; }
        
        [Required]
        public Guid SectionId { get; set; }
        public Tag Section { get; set; }
    }
}
