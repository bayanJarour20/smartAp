using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Constants;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using SmartStart.Model.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Model.Main
{
    public class ExamDocument : BaseEntity<Guid>
    {
        [Required]
        public Guid ExamId { get; set; }
        public Exam Exam { get; set; }


        [Required]
        public Guid DocumentId { get; set; }
        public Document Document { get; set; }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.LongString)]
        public string Note { get; set; }
    }
}
