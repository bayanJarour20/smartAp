using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.SettingDto;
using SmartStart.Repository.Invoice.CodeService;
using SmartStart.Repository.PointOfSale.PointOfSaleService;
using SmartStart.Repository.Setting.AdvertisementService;
using SmartStart.Repository.Setting.NotificationService;
using SmartStart.Repository.Setting.SettingService;
using SmartStart.Security;
using SmartStart.SharedKernel.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{
    [ApiController]

    public class SettingController : ElControllerBase<Guid>
    {
        private readonly ISettingRepository settingRepository;
        private readonly ICodeRepository codeRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly IPointOfSaleRepository pointOfSaleRepository;
        private readonly IAdvertisementRepository advertisementRepository;
        public SettingController(ICodeRepository codeRepository, 
                                 INotificationRepository notificationRepository, 
                                 IPointOfSaleRepository pointOfSaleRepository, 
                                 IAdvertisementRepository advertisementRepository, 
                                 ISettingRepository settingRepository)
        {
            this.codeRepository = codeRepository;
            this.notificationRepository = notificationRepository;
            this.pointOfSaleRepository = pointOfSaleRepository;
            this.advertisementRepository = advertisementRepository;
            this.settingRepository = settingRepository;
        }

        [Route("api/[controller]/[action]")]
        [Route("[controller]/[action]")]
        [Route("api/apiSettings/[action]")]
        [Route("api/apisetting/[action]")]
        [Route("api Settings/[action]")]
        [Route("api setting/[action]")]
        [HttpGet]
        public async Task<IActionResult> CheckAppVersion(string CurrentAppVersion) => await settingRepository.CheckAppVersion(CurrentAppVersion).ToJsonResultAsync();

        [Route("api/[controller]/[action]")]
        [ElAuthorizeDistributed(SmartStartRoles.User)]
        [HttpPost]
        public async Task<IActionResult> Add(SettingDto dto) => await settingRepository.AddAsync(dto).ToJsonResultAsync();

        [Route("api/[controller]/[action]")]
        [ElAuthorizeDistributed(SmartStartRoles.User)]
        [HttpPost]
        public async Task<IActionResult> Modify(SettingDto dto) => await settingRepository.ModifyAsync(dto).ToJsonResultAsync();

        [Route("api/[controller]/[action]")]
        [ElAuthorizeDistributed(SmartStartRoles.User)]
        [HttpPost]
        public async Task<IActionResult> Update(SettingDto dto) => await settingRepository.UpdateAsync(dto).ToJsonResultAsync();

        [Route("api/[controller]/[action]")]
        [ElAuthorizeDistributed(SmartStartRoles.User)]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id) => await settingRepository.DeleteAsync(id).ToJsonResultAsync();

        [Route("api/[controller]/[action]")]
        [ElAuthorizeDistributed(SmartStartRoles.User)]
        [HttpGet]
        public async Task<IActionResult> Fetch() => (await codeRepository.GetCodes(Key.Value)).Collect(await notificationRepository.GetNotifications(Key.Value), 
                                                                                                       await pointOfSaleRepository.GetPointOfSales(), 
                                                                                                       await advertisementRepository.GetAdvertisement())
                                                                         .Into((codes, notifications, pointOfSales, advertisement) => new {
                                                                             codes = codes.Result,
                                                                             notifications = notifications.Result,
                                                                             pointOfSales = pointOfSales.Result,
                                                                             advertisement = advertisement.Result
                                                                         }).ToJsonResult();
        [Route("api/[controller]/[action]")]
        [ElAuthorizeDistributed(SmartStartRoles.User)]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id) => await settingRepository.GetByIdAsync(id).ToJsonResultAsync();

    }
}
