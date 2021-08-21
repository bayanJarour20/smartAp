using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.GeneralDto;
using SmartStart.Repository.Main.GeneralServices;
using SmartStart.Security;
using SmartStart.SharedKernel.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GeneralController : ElControllerBase<Guid, IGeneralRepository>
    {
        public GeneralController(IGeneralRepository repository) : base(repository) { }

        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.User)]
        public async Task<IActionResult> Remaining() => await repository.GetRemaining(Key.Value).ToJsonResultAsync();

        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.User)]
        public async Task<IActionResult> SetSelected(SelectedDto selectedDto) => await repository.SetSelected(selectedDto, Key.Value).ToJsonResultAsync();
    }
}
