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
using System.Text;

namespace SmartStart.Model.Shared
{
    public class Document : BaseEntity<Guid>, INominal
    {
        public Document()
        {
            QuestionDocuments = new HashSet<QuestionDocument>();
            ExamDocuments = new HashSet<ExamDocument>();
        }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.NounString)]
        public string Name { get; set; }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.MediumString)]
        [Required]
        public string Path { get; set; }

        [ColumnDataType(DataBaseTypes.TINYINT)]
        public DocumentTypes Type { get; set; }

        public ICollection<QuestionDocument> QuestionDocuments { get; set; }
        public ICollection<ExamDocument> ExamDocuments { get; set; }
    }
}
