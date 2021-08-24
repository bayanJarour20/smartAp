using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.PackageDto;
using SmartStart.Repository.Invoice.PackageService;
using SmartStart.Security;
using SmartStart.SharedKernel.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ElAuthorizeDistributed(SmartStartRoles.Admin)]
    public class PackageController : ElControllerBase<Guid, IPackageRepository>
    {
        public PackageController(IPackageRepository repository) : base(repository) { }

        [HttpGet]
        public async Task<IActionResult> GetAll() => await repository.GetAll().ToJsonResultAsync();

        [HttpGet]
        public async Task<IActionResult> Details([Required] Guid id) => await repository.Details(id).ToJsonResultAsync();

        [HttpPost]
        public async Task<IActionResult> Add(PackageSubjectDto dto) => await repository.Add(dto).ToJsonResultAsync();

        [HttpPut]
        public async Task<IActionResult> Update(PackageSubjectDto dto) => await repository.Update(dto).ToJsonResultAsync();

        [HttpDelete]
        public async Task<IActionResult> RemovePackage([Required] Guid id) => await repository.RemovePackage(id).ToJsonResultAsync();

        [HttpDelete]
        public async Task<IActionResult> RemovePackages([Required] List<Guid> ids) => await repository.RemovePackages(ids).ToJsonResultAsync();


        [HttpPost]
        public async Task<IActionResult> Init() => await repository.Init().ToJsonResultAsync();
    }
}
