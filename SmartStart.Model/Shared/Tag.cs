using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Constants;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using Elkood.Web.Infrastructure.ModelEntity.Interface;
using SmartStart.Model.Main;
using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartStart.Model.Shared
{
    public class Tag : BaseEntity<Guid>, INominal
    {
        public Tag()
        {
            ExamTags = new HashSet<ExamTag>();
            QuestionTags = new HashSet<QuestionTag>();
            SubjectTags = new HashSet<SubjectTag>();
        }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.NounString)]
        [Required]
        public string Name { get; set; }

        [ColumnDataType(DataBaseTypes.TINYINT)]
        public TagTypes Type { get; set; }

        public ICollection<ExamTag> ExamTags { get; set; }
        public ICollection<QuestionTag> QuestionTags { get; set; }
        public ICollection<SubjectTag> SubjectTags { get; set; }

        [InverseProperty("Semester")]
        public ICollection<SubjectFaculty> SubjectFacultysSemester { get; set; }

        [InverseProperty("Section")]
        public ICollection<SubjectFaculty> SubjectFacultysSection { get; set; }

    }
}
