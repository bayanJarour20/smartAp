using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.AccountDto;
using SmartStart.Repository.Security.UserService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    //[ElAuthorizeDistributed(TarafouaRoles.Admin)]
    public class UserController : ElControllerBase<Guid, IUserRepository>
    {
        public UserController(IUserRepository repository) : base(repository) { }

        [HttpGet]
        public async Task<IActionResult> GetAllUser() => await repository.GetAllUser().ToJsonResultAsync();
        [HttpPut]
        public async Task<IActionResult> Update(AppUserDto account) => await repository.Update(account).ToJsonResultAsync();

        [HttpPost]
        public async Task<IActionResult> Create(AppUserDto account) => await repository.Create(account).ToJsonResultAsync();

        [HttpDelete]
        public async Task<IActionResult> Delete([Required] Guid id) => await repository.Delete(id).IntoAsync(o => o).ToJsonResultAsync();

        [HttpDelete]
        public async Task<IActionResult> DeleteRange([Required] IEnumerable<Guid> ids) => await repository.DeleteRange(ids).IntoAsync(n => n).ToJsonResultAsync();

        [HttpPut("{id}")]
        public async Task<IActionResult> Block(Guid id) => await repository.Block(id).IntoAsync(o => o).ToJsonResultAsync();

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid id) => await repository.Details(id).ToJsonResultAsync();
    }
}
