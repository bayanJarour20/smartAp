using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.UniverstiyDto;
using SmartStart.Repository.General.UniversityService;
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
    [ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
    public class UniversityController : ElControllerBase<Guid, IUniversityRepository>
    {
        public UniversityController(IUniversityRepository repository) : base(repository) { }

        [HttpGet]
        public async Task<IActionResult> GetAll() => await repository.GetAll().ToJsonResultAsync();

        [HttpPost]
        public async Task<IActionResult> Add(UniversityDto universityDto) => await repository.Add(universityDto).ToJsonResultAsync();

        [HttpPut]
        public async Task<IActionResult> Update(UniversityDto universityDto) => await repository.Update(universityDto).ToJsonResultAsync();

        [HttpDelete]
        public async Task<IActionResult> Delete([Required] Guid id) => await repository.Delete(id).IntoAsync(n => n).ToJsonResultAsync();

        [HttpDelete]
        public async Task<IActionResult> DeleteRange([Required] IEnumerable<Guid> ids) => await repository.DeleteRange(ids).IntoAsync(n => n).ToJsonResultAsync();

    }
}
