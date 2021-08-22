using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.CityDto;
using SmartStart.Repository.General.CityService;
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
    public class CityController : ElControllerBaseGeneral<Guid, ICityRepository, CityDto>
    {
        public CityController(ICityRepository repository) : base(repository) { }

        [HttpDelete]
        public async Task<IActionResult> DeleteRange([Required] IEnumerable<Guid> ids) => await repository.DeleteRange(ids).IntoAsync(n => n).ToJsonResultAsync();
    }
}
