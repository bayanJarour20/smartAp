using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Constants;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStart.Model.Main
{
    public class Question : BaseEntity<Guid>
    {
        public Question()
        {
            QuestionTags = new HashSet<QuestionTag>();
            ExamQuestions = new HashSet<ExamQuestion>();
            Answers = new HashSet<Answer>();
            QuestionDocuments = new HashSet<QuestionDocument>();
        }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.LongString)]
        public string Title { get; set; }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.LongString)]
        public string Hint { get; set; }

        [ColumnDataType(DataBaseTypes.TINYINT)]
        public QuestionTypes QuestionType { get; set; }

        [ColumnDataType(DataBaseTypes.TINYINT)]
        public AnswerTypes AnswerType { get; set; }

        [ColumnDataType(DataBaseTypes.BIT)]
        public bool IsCorrected { get; set; }


        public ICollection<QuestionTag> QuestionTags { get; set; }
        public ICollection<ExamQuestion> ExamQuestions { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public ICollection<QuestionDocument> QuestionDocuments { get; set; }
    }
}
