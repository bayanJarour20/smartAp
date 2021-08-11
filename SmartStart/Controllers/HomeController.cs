using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartStart.Repository.Shared.HomeService;
using SmartStart.Security;
using SmartStart.SharedKernel.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{
    [ApiController]
    [ElAuthorizeDistributed(SmartStartRoles.Admin)]
    public class HomeController : ElControllerBase<Guid, IHomeRepository>
    {
        public HomeController(IHomeRepository repository) : base(repository) { }

        [Route("api/[controller]")]
        [HttpGet]
        public async Task<IActionResult> Index() => await repository.Home().ToJsonResultAsync();
    }
}
