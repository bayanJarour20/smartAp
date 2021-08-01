using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartStart.Model.Main
{
    public class ExamQuestion : BaseEntity<Guid>
    {
        [Required]
        public Guid ExamId { get; set; }
        public Exam Exam { get; set; }


        [Required]
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }


        [ColumnDataType(DataBaseTypes.SMALLINT)]
        public short Order { get; set; }
    }
}
