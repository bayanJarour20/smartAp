using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.CityDto;
using SmartStart.Repository.General.CityService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]

    public class CityController : ElControllerBase<Guid, ICityRepositroy>
    {

        public CityController(ICityRepositroy repository) : base(repository) { }

        [HttpGet]
        public async Task<IActionResult> GetAllCites() => await repository.GetCities().ToJsonResultAsync();
    }
}
