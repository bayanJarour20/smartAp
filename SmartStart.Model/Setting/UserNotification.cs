using Elkood.Web.Infrastructure.ModelEntity.Base;
using SmartStart.Model.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartStart.Model.Setting
{
    public class UserNotification : BaseEntity<Guid>
    {
        [Required]
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        [Required]
        public Guid NotificationId { get; set; }
        public Notification Notification { get; set; }
    }
}
