using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Constants;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStart.Model.Setting
{
    public class Setting : BaseEntity<Guid>
    {
        [ColumnDataType(DataBaseTypes.VARCHAR, TypeConstants.ShortString)]
        public string AppVersion { get; set; }
        [ColumnDataType(DataBaseTypes.VARCHAR, TypeConstants.ShortString)]
        public string SupportedAppVersions { get; set; }
    }
}
