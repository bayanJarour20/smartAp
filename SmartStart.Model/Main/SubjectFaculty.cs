using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Enum;
using SmartStart.Model.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Model.Main
{
    public class SubjectFaculty
    {
        [Required]
        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }

        [Required]
        public Guid QuestionId { get; set; }
        public Faculty Faculty { get; set; }

        [ColumnDataType(DataBaseTypes.SMALLINT)]
        public int Year { get; set; }
    }
}
