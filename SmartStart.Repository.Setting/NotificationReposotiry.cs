using Elkood.Web.Service.BoundedContext;
using SmartStart.Model.Setting;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Setting
{
    public class NotificationReposotiry : ElRepository<SmartStartDbContext, Guid, Notification>, INotificationRepository
    {
        public NotificationReposotiry(SmartStartDbContext context) : base(context)
        {
        }


    }
}
