using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using SmartStart.DataTransferObject.NotificationDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Setting.NotificationService
{
    public interface INotificationRepository : IElRepository
    {
        Task<OperationResult<IEnumerable<NotificationToMeDto>>> GetNotifications(Guid id);
        Task<OperationResult<IEnumerable<NotificationUsersDto>>> GetAll();
        Task<OperationResult<NotificationUsersDto>> Add(NotificationUsersDto notificationDto);
        Task<OperationResult<bool>> Delete(Guid id);
    }
}
