using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CodeController : ElControllerBase<Guid, ICodeRepository>
    {
        public CodeController(ICodeRepository repository) : base(repository) { }

        [HttpGet, ElAuthorizeDistributed(TarafouaRoles.User)]
        public async Task<IActionResult> GetCodes() => await repository.GetCodes(Key.Value).ToJsonResultAsync();

        [HttpPost, ElAuthorizeDistributed(TarafouaRoles.User)]
        public async Task<IActionResult> CheckCode([FromBody] string hash) => await repository.CheckCode(hash).ToJsonResultAsync();

        [HttpPost, ElAuthorizeDistributed(TarafouaRoles.User)]
        public async Task<IActionResult> ActivateCode([FromBody] string hash) => await repository.ActivateCode(Key.Value, hash).IntoAsync(o => o).ToJsonResultAsync();

        [HttpPost, ElAuthorizeDistributed(TarafouaRoles.User)]
        public async Task<IActionResult> ActivateCodeV2(ActivateCodeViewModel activate) => await repository.ActivateCodeV2(activate.Hash, activate.SubjectIds).IntoAsync(o => o).ToJsonResultAsync();


        [HttpGet, ElAuthorizeDistributed(TarafouaRoles.Admin)]
        public async Task<IActionResult> GetAll() => await repository.GetAll().ToJsonResultAsync();

        [HttpPost, ElAuthorizeDistributed(TarafouaRoles.Admin)]
        public async Task<IActionResult> Generate(CodeGenerateDto dto) => await repository.Generate(dto).ToJsonResultAsync();

        [HttpPut, ElAuthorizeDistributed(TarafouaRoles.Admin)]
        public async Task<IActionResult> Update(CodeGenerateDto dto) => await repository.Update(dto).ToJsonResultAsync();

        [HttpDelete, ElAuthorizeDistributed(TarafouaRoles.Admin)]
        public async Task<IActionResult> Delete([Required] Guid id) => await repository.Delete(id).IntoAsync(o => o).ToJsonResultAsync();
    }
}
