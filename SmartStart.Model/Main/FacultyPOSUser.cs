﻿using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using SmartStart.Model.General;
using SmartStart.Model.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Model.Main
{
    public class FacultyPOSUser : BaseEntity<Guid>
    {
        [ColumnDataType(DataBaseTypes.BIT)]
        public bool DefaultSelected { get; set; }

        [Required]
        public Guid FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        [Required]
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
