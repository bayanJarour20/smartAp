using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.NotificationDto;
using SmartStart.Model.Setting;
using SmartStart.SharedKernel.Enums;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartStart.Repository.Setting.NotificationService
{
    public class NotificationRepository : ElRepository<SmartStartDbContext, Guid, Notification>, INotificationRepository
    {
        private readonly IHttpClientFactory httpClient;

        public NotificationRepository(SmartStartDbContext context, IHttpClientFactory httpClient) : base(context)
        {
            this.httpClient = httpClient;
        }

        public async Task<OperationResult<IEnumerable<NotificationToMeDto>>> GetNotifications(Guid id)
            => await RepositoryHandler(_getNotifications(id));

        public async Task<OperationResult<IEnumerable<NotificationUsersDto>>> GetAll()
            => await RepositoryHandler(_getAll());

        //public async Task<OperationResult<NotificationUsersDto>> Add(NotificationUsersDto notificationDto)
        //    => await RepositoryHandler(_add(notificationDto));

        public async Task<OperationResult<bool>> Delete(Guid id)
            => await RepositoryHandler(_delete(id));

        private Func<OperationResult<IEnumerable<NotificationUsersDto>>, Task<OperationResult<IEnumerable<NotificationUsersDto>>>> _getAll()
           => async operation =>
           {
               var allNotification = await Query.Select(notification => new NotificationUsersDto
               {
                   Id = notification.Id,
                   Title = notification.Title,
                   Body = notification.Body,
                   Date = notification.Date,
                   NotificationType = notification.Type,
                   UserIds = notification.UserNotifications.Select(un => un.AppUserId),
               }).ToListAsync();

               return operation.SetSuccess(allNotification);
           };

        private Func<OperationResult<IEnumerable<NotificationToMeDto>>, Task<OperationResult<IEnumerable<NotificationToMeDto>>>> _getNotifications(Guid id)
            => async operation =>
            {
                var notifications = await Query.Include(notification => notification.UserNotifications).Where(notification => notification.Type == NotificationTypes.User &&
                                                                       (!notification.UserNotifications.Any() ||
                                                                       notification.UserNotifications.Any(usernotification => usernotification.AppUserId == id)
                                                                        ))
                                               .Select(notification => new NotificationToMeDto()
                                               {
                                                   Id = notification.Id,
                                                   Title = notification.Title,
                                                   Body = notification.Body,
                                                   Date = notification.Date,
                                                   NotificationType = NotificationTypes.User,
                                                   IsToMe = notification.UserNotifications.Any()
                                               }).ToListAsync();

                return operation.SetSuccess(notifications);
            };

        //private Func<OperationResult<NotificationUsersDto>, Task<OperationResult<NotificationUsersDto>>> _add(NotificationUsersDto notificationDto)
        //    => async operation =>
        //    {

        //        using (var transaction = await Context.Database.BeginTransactionAsync())
        //        {

        //            try
        //            {
        //                var notification = new Notification()
        //                {
        //                    Title = notificationDto.Title,
        //                    Body = notificationDto.Body,
        //                    Date = notificationDto.Date.ToLocalTime(),
        //                    Type = notificationDto.NotificationType,
        //                    UserNotifications = notificationDto.UserIds?.Select(id => new UserNotification
        //                    {
        //                        AppUserId = id,
        //                    }).ToList(),
        //                };

        //                await Context.AddAsync(notification);
        //                await Context.SaveChangesAsync();
        //                notificationDto.Id = notification.Id;


        //                var res = await SendNotification(notificationDto);
        //                if (!res.IsSuccess)
        //                {
        //                    await transaction.RollbackAsync();
        //                    if (res.HasException())
        //                        return operation.SetException(res.Exception);
        //                    return operation.SetFailed(res.Message);
        //                }

        //                await transaction.CommitAsync();
        //                return operation.SetSuccess(notificationDto);

        //            }
        //            catch (Exception e)
        //            {
        //                await transaction.RollbackAsync();
        //                return operation.SetException(e);
        //            }
        //        }

        //    };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _delete(Guid id)
            => async operation =>
            {
                await Context.SoftDeleteTraversalAsync((Expression<Func<Notification, bool>>)(noti => noti.Id == id), noti => noti.UserNotifications);
                int rowEffected = await Context.SaveChangesDeletedAsync();
                return operation.SetSuccess(true, $"{nameof(rowEffected)}: {rowEffected}");
            };

        public async Task<OperationResult<bool>> SendNotification(NotificationUsersDto notificationDto)
        {
            try
            {
                string applicationID = String.Empty;
                //string senderId = String.Empty;
                string deviceId = "";

                // send to user
                applicationID = "AAAAtfcMGl8:APA91bF42b3aDoLzGo-uADNi7N0d3DOo-3OdeF8_v6iAu9-a9d8YP2EhX2q6hVK_CVk2NxsUV-r4G0Mxvzug_E7blWvSKuqUF_kcVtT2aFsGCkjW81L7-rYwEmoD3rvD7MgTDZWG8caD";
                //senderId = "24555187283";
                if (notificationDto.NotificationType == NotificationTypes.Seller)
                {
                    applicationID = "AAAA5X-gqQ8:APA91bFWF3sFyhVYGTd7C0SmLsFGMZuzlX8gOYlZlx76u79BhYD1c9VMbqMZj4KpQCtZVmGMvUPiFDT8VOMbQ89R4dGE0r1juQXZ44yXh-wguxte2zCBLuFJJcEpXaFPm_mcY3XlKRDG";
                }

                if (!(notificationDto.UserIds?.Any() ?? false))
                {
                    deviceId = "/topics/all";
                }
                else
                {
                    //TODO get sudetn dev id 
                    //deviceId = student.DeviceToken;
                }

                using (var response = await HttpFCMSender(applicationID, deviceId, notificationDto))
                {

                    if (response.IsSuccessStatusCode)
                        return new OperationResult<bool>().SetSuccess(true);

                    return new OperationResult<bool>().SetFailed(await response.RequestMessage.Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                return new OperationResult<bool>().SetException(e);
            }
        }

        private async Task<HttpResponseMessage> HttpFCMSender(string applicationID, string deviceId, NotificationDto notificationDto)
        {

            var _fcmHttp = httpClient.CreateClient("fcm");

            var Note = new
            {
                to = deviceId,
                //priority = "high",
                notification = new
                {
                    title = notificationDto.Title,
                    body = notificationDto.Body,
                }
            };

            var json = JsonSerializer.Serialize(Note);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            _fcmHttp.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"key={applicationID}");

            _fcmHttp.DefaultRequestHeaders.TryAddWithoutValidation("Sender", $"id={applicationID}");

            return await _fcmHttp.PostAsync("fcm/send", httpContent);
        }

    }
}
