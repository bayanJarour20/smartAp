using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.SettingDto;
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

        public SettingController(ISettingRepository settingRepository)
        {
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
        public async Task<IActionResult> GetAll() => await settingRepository.FetchAsync().ToJsonResultAsync();

        [Route("api/[controller]/[action]")]
        [ElAuthorizeDistributed(SmartStartRoles.User)]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id) => await settingRepository.GetByIdAsync(id).ToJsonResultAsync();

    }
}
