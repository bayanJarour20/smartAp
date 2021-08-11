using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Elkood.Web.MVC.Security.Attribute;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.AdvertisementDto;
using SmartStart.Repository.Setting.AdvertisementService;
using SmartStart.Security;
using SmartStart.SharedKernel.Security;
using SmartStart.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdvertisementController : ElControllerBaseGeneral<Guid, IAdvertisementRepository, AdvertisementDto>
    {
        public AdvertisementController(IAdvertisementRepository repository) : base(repository) { }

        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public override Task<IActionResult> Fetch() => base.Fetch();
        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public override Task<IActionResult> GetById(Guid id) => base.GetById(id);
        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public override Task<IActionResult> Add(AdvertisementDto dto) => base.Add(dto);
        [HttpPut, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public override Task<IActionResult> Update(AdvertisementDto dto) => base.Update(dto);
        [HttpPut, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public override Task<IActionResult> Modify(AdvertisementDto dto) => base.Modify(dto);
        [HttpDelete, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public override Task<IActionResult> Delete(Guid id) => base.Delete(id);

        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [DisableRequestSizeLimit]
        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> Upload([FromForm] UploadAdvertisementViewModel upload)
           => await repository.Upload(upload, upload.File).ToJsonResultAsync();

        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.User)]
        public async Task<IActionResult> GetAdvertisement()
            => await repository.GetAdvertisement().ToJsonResultAsync();

    }
}
