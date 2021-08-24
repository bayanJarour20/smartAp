using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.NotificationDto;
using SmartStart.Model.Setting;
using SmartStart.SharedKernel.Enums;
using SmartStart.SharedKernel.ExtensionMethods;
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
    [ElRepository]
    public class NotificationRepository : ElRepository<SmartStartDbContext, Guid, Notification>, INotificationRepository
    {
        private readonly IHttpClientFactory httpClient;

        public NotificationRepository(SmartStartDbContext context, IHttpClientFactory httpClient) : base(context)
        {
            this.httpClient = httpClient;
        }

        public async Task<OperationResult<NotificationTupleDto>> GetNotifications(Guid id)
            => await RepositoryHandler(_getNotifications(id));

        public async Task<OperationResult<IEnumerable<NotificationUsersDto>>> GetAll()
            => await RepositoryHandler(_getAll());

        public async Task<OperationResult<NotificationUsersDto>> Add(NotificationUsersDto notificationDto)
            => await RepositoryHandler(_add(notificationDto));

        public async Task<OperationResult<bool>> Delete(Guid id)
            => await RepositoryHandler(_delete(id));

        public async Task<OperationResult<bool>> DeleteRange(IEnumerable<Guid> ids)
           => await RepositoryHandler(_deleteRange(ids));

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

        private Func<OperationResult<NotificationTupleDto>, Task<OperationResult<NotificationTupleDto>>> _getNotifications(Guid id)
            => async operation =>
            {
                var notificationsAll = await Query.Include(notification => notification.UserNotifications)
                                                  .Where(notification => notification.Type == NotificationTypes.User
                                                                      && (!notification.UserNotifications.Any()
                                                                      || notification.UserNotifications.Any(usernotification => usernotification.AppUserId == id)))
                                                  .Select(notification => new NotificationToMeDto()
                                                  {
                                                      Id = notification.Id,
                                                      Title = notification.Title,
                                                      Body = notification.Body,
                                                      Date = notification.Date,
                                                      NotificationType = NotificationTypes.User,
                                                      IsToMe = notification.UserNotifications.Any()
                                                  }).ToListAsync();

                var notificationsToday = await Query.Include(notification => notification.UserNotifications)
                                                    .Where(notification => notification.Type == NotificationTypes.User
                                                                        && notification.Date.Date == DateTime.Now.Date
                                                                        && (!notification.UserNotifications.Any()
                                                                        || notification.UserNotifications.Any(usernotification => usernotification.AppUserId == id)))
                                                    .Select(notification => new NotificationToMeDto()
                                                    {
                                                        Id = notification.Id,
                                                        Title = notification.Title,
                                                        Body = notification.Body,
                                                        Date = notification.Date,
                                                        NotificationType = NotificationTypes.User,
                                                        IsToMe = notification.UserNotifications.Any()
                                                    }).ToListAsync();

                return operation.SetSuccess(new NotificationTupleDto 
                {
                    NotificationAll = notificationsAll,
                    NotificationToday = notificationsToday
                });
            };

        private Func<OperationResult<NotificationUsersDto>, Task<OperationResult<NotificationUsersDto>>> _add(NotificationUsersDto notificationDto)
            => async operation =>
            {

                using (var transaction = await Context.Database.BeginTransactionAsync())
                {

                    try
                    {
                        var notification = new Notification()
                        {
                            Title = notificationDto.Title,
                            Body = notificationDto.Body,
                            Date = notificationDto.Date.ToLocalTime(),
                            Type = notificationDto.NotificationType,
                            UserNotifications = notificationDto.UserIds?.Select(id => new UserNotification
                            {
                                AppUserId = id,
                            }).ToList(),
                        };

                        await Context.AddAsync(notification);
                        await Context.SaveChangesAsync();
                        notificationDto.Id = notification.Id;


                        var res = await SendNotification(notificationDto);
                        if (!res.IsSuccess)
                        {
                            await transaction.RollbackAsync();
                            if (res.HasException())
                                return operation.SetException(res.Exception);
                            return operation.SetFailed(res.Message);
                        }

                        await transaction.CommitAsync();
                        return operation.SetSuccess(notificationDto);

                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        return operation.SetException(e);
                    }
                }

            };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _delete(Guid id)
            => async operation =>
            {
                await Context.SoftDeleteTraversalAsync((Expression<Func<Notification, bool>>)(noti => noti.Id == id), noti => noti.UserNotifications);
                int rowEffected = await Context.SaveChangesDeletedAsync();
                return operation.SetSuccess(true, $"{nameof(rowEffected)}: {rowEffected}");
            };



        

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _deleteRange(IEnumerable<Guid> ids)
           => async operation =>
           {

               var NotificationModels = await Query.Where(e => ids.Contains(e.Id)).Include(e => e.UserNotifications).ToListAsync();


               foreach(Notification item in NotificationModels)
               {
                   await Context.SoftDeleteTraversalAsync((Expression<Func<Notification, bool>>)(noti => noti.Id == item.Id ), noti => noti.UserNotifications);
                   //await Context.SaveChangesAsync();
               }


               //await Context.SaveChangesAsync();

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
                applicationID = "AAAAHzROU30:APA91bH1VZu_L9RVY6XeDdw4zfVOn0fl-0g0Pp-UdkBMWDSm_HlHQ42c4HP90JViiW_msm9dnsndkmheoe-FNRZNghq-qTnwLNXWRfQx2F-ykm5zlw63uBB8akVCoXrX7eRFLVlDYK3F";
                //senderId = "24555187283";
                if (notificationDto.NotificationType == NotificationTypes.Seller)
                {
                    applicationID = "AAAAlcnPf1U:APA91bHkhXRsgPpxMV3PV2KmasV2Es_eHbuezoeWET76JRQahFanVwtTipDKO3HjCTtzseq_bZCXxvCiPTU1j-dBmkxSjgxG187lKGownV5CkcBWdbt9eJ2_s-zz18C32Zms94XEwa82";
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
