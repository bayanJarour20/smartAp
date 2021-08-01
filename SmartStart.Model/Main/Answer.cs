using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartStart.Model.Main
{
    public class Answer : BaseEntity<Guid>
    {
        public Answer()
        {
            Answers = new HashSet<Answer>();
        }

        [ColumnDataType(DataBaseTypes.NVARCHAR)]
        public string Title { get; set; }

        [ColumnDataType(DataBaseTypes.BIT)]
        public bool IsCorrect { get; set; }

        [ColumnDataType(DataBaseTypes.NVARCHAR)]
        public string Option { get; set; }

        [ColumnDataType(DataBaseTypes.DATE)]
        public DateTime? CorrectionDate { get; set; }


        [Required]
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }


        public Guid? CorrectAnswerId { get; set; }
        public Answer CorrectAnswer { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}
