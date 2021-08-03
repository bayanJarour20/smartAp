using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Constants;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using Elkood.Web.Infrastructure.ModelEntity.Interface;
using SmartStart.Model.Business;
using SmartStart.Model.General;
using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartStart.Model.Main
{
    public class Subject : BaseEntity<Guid>, INominal
    {
        public Subject()
        {
            SubjectTags = new HashSet<SubjectTag>();
            Exams = new HashSet<Exam>();
            SubjectAppUsers = new HashSet<SubjectAppUser>();
        }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.NounString)]
        [Required]
        public string Name { get; set; }


        [ColumnDataType(DataBaseTypes.TINYINT)]
        public SubjectTypes Type { get; set; }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.MediumString)]
        public string ImagePath { get; set; }

        [ColumnDataType(DataBaseTypes.NVARCHAR)]
        public string Description { get; set; }

        [Required]
        public Guid FacultyId { get; set; }
        public Faculty Faculty { get; set; }


        [Required]
        public bool IsFree { get; set; }

        public ICollection<SubjectTag> SubjectTags { get; set; }
        public ICollection<Exam> Exams { get; set; }
        public ICollection<SubjectAppUser> SubjectAppUsers { get; set; }
        public ICollection<PackageSubject> Packages{ get; set; }
    }
}
