using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Constants;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStart.Model.Setting
{
    public class Notification : BaseEntity<Guid>
    {
        public Notification()
        {
            UserNotifications = new HashSet<UserNotification>();
        }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.NounString)]
        public string Title { get; set; }
        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.LongString)]
        public string Body { get; set; }
        [ColumnDataType(DataBaseTypes.DATETIME2)]
        public DateTime Date { get; set; }
        [ColumnDataType(DataBaseTypes.BIT)]
        public bool HasReaded { get; set; }
        [ColumnDataType(DataBaseTypes.TINYINT)]
        public NotificationTypes Type { get; set; }

        public ICollection<UserNotification> UserNotifications { get; set; }
    }
}
