using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Elkood.Web.MVC.Security.Attribute;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.NotificationDto;
using SmartStart.Repository.General.CityService;
using SmartStart.Repository.Setting;
using SmartStart.Repository.Setting.NotificationService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NotificationController : ElControllerBase<Guid, INotificationRepository>
    {
        public NotificationController(INotificationRepository notificationRepository) : base(notificationRepository) { }

        [HttpGet]
        public async Task<IActionResult> GetNotifications() => await repository.GetNotifications(Key.Value).ToJsonResultAsync();

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return await repository.GetAll().ToJsonResultAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Add(NotificationUsersDto notification) => await repository.Add(notification).ToJsonResultAsync();

        [HttpDelete]
        public async Task<IActionResult> Delete([Required] Guid id) => await repository.Delete(id).IntoAsync(n => n).ToJsonResultAsync();

        [HttpDelete]
        public async Task<IActionResult> DeleteRange([Required] IEnumerable<Guid> ids) => await repository.DeleteRange(ids).IntoAsync(n => n).ToJsonResultAsync();
    }
}
