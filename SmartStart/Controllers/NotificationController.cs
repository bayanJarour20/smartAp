using Elkood.Web.MVC;
using Microsoft.AspNetCore.Mvc;
using SmartStart.Repository.General.CityService;
using SmartStart.Repository.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NotificationController : ElControllerBase<Guid, INotificationRepository>
    {

        public NotificationController(INotificationRepository notificationRepository) : base(notificationRepository) { }


    }
}
