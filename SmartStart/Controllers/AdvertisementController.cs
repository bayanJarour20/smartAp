using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Elkood.Web.MVC.Security.Attribute;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.AdvertisementDto;
using SmartStart.Repository.Setting.AdvertisementService;
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

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] UploadAdvertisementViewModel upload)
           => await repository.Upload(upload, upload.File).ToJsonResultAsync();

        [HttpGet]
        public async Task<IActionResult> GetAdvertisement()
            => await repository.GetAdvertisement().ToJsonResultAsync();
    }
}
