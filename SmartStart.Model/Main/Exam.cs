using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Constants;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using Elkood.Web.Infrastructure.ModelEntity.Interface;
using SmartStart.Model.Business;
using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartStart.Model.Main
{
    public class Exam : BaseEntity<Guid>, INominal
    {

        public Exam()
        {
            ExamTags = new HashSet<ExamTag>();
            ExamQuestions = new HashSet<ExamQuestion>();
            PackageSubjects = new HashSet<PackageSubjectFaculty>();
        }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.MediumString)]
        [Required]
        public string Name { get; set; }

        [ColumnDataType(DataBaseTypes.SMALLINT)]
        public int Year { get; set; }

        [ColumnDataType(DataBaseTypes.TINYINT)]
        public TabTypes Type { get; set; }

        /// <summary>
        /// Default Price
        /// </summary>
        [ColumnDataType(DataBaseTypes.FLOAT)]
        public double Price { get; set; }

        [ColumnDataType(DataBaseTypes.BIT)]
        public bool IsFree { get; set; }


        [Required]
        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }


        public ICollection<ExamTag> ExamTags { get; set; }
        public ICollection<ExamQuestion> ExamQuestions { get; set; }
        public ICollection<PackageSubjectFaculty> PackageSubjects { get; set; }
        public ICollection<ExamDocument> ExamDocuments { get; set; }
    }
}
