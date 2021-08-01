using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Constants;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using SmartStart.Model.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartStart.Model.Setting
{
    public class Feedback : BaseEntity<Guid>
    {

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.NounString)]
        public string Title { get; set; }
        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.LongString)]
        public string Body { get; set; }
        [ColumnDataType(DataBaseTypes.NVARCHAR)]
        public string Reply { get; set; }
        [ColumnDataType(DataBaseTypes.DATETIME2)]
        public DateTime? ReplyDate { get; set; }
        [Required]
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
