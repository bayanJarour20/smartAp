using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.CityDto;
using SmartStart.Repository.General.CityService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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


        [HttpPost]
        public async Task<IActionResult> Add(CityDto cityDto) => await repository.Add(cityDto).ToJsonResultAsync();

        [HttpPut]
        public async Task<IActionResult> Update(CityDto cityDto) => await repository.Update(cityDto).ToJsonResultAsync();

        [HttpDelete]
        public async Task<IActionResult> Delete([Required] Guid id) => await repository.Delete(id).IntoAsync(n => n).ToJsonResultAsync();

        [HttpDelete]
        public async Task<IActionResult> DeleteRange([Required] IEnumerable<Guid> ids) => await repository.DeleteRange(ids).IntoAsync(n => n).ToJsonResultAsync();

    }
}
