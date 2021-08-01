using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Constants;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Text;

namespace SmartStart.Model.Main
{
    public class QuestionDocument : BaseEntity<Guid>
    {
        [Required]
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }


        [Required]
        public Guid DocumentId { get; set; }
        public Document Document { get; set; }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.LongString)]
        public string Note { get; set; }
    }
}
