using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.CodeDto;
using SmartStart.Repository.Invoice.CodeService;
using SmartStart.Security;
using SmartStart.SharedKernel.Security;
using SmartStart.ViewModels.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CodeController : ElControllerBase<Guid, ICodeRepository>
    {
        public CodeController(ICodeRepository repository) : base(repository) { }

        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.User)]
        public async Task<IActionResult> GetCodes() => await repository.GetCodes(Key.Value).ToJsonResultAsync();

        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.User)]
        public async Task<IActionResult> CheckCode([FromBody] string hash) => await repository.CheckCode(hash).ToJsonResultAsync();

        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.User)]
        public async Task<IActionResult> ActivateCode([FromBody] string hash) => await repository.ActivateCode(Key.Value, hash).IntoAsync(o => o).ToJsonResultAsync();

        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.User)]
        public async Task<IActionResult> ActivateCodeV2(ActivateCodeViewModel activate) => await repository.ActivateCodeV2(activate.Hash, activate.subjectFacultyIds).IntoAsync(o => o).ToJsonResultAsync();

        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> GetAll() => await repository.GetAll().ToJsonResultAsync();

        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> Generate(CodeGenerateDto dto) => await repository.Generate(dto).ToJsonResultAsync();

        [HttpPut, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> Update(CodeGenerateDto dto) => await repository.Update(dto).ToJsonResultAsync();

        [HttpDelete, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> Delete([Required] Guid id) => await repository.Delete(id).IntoAsync(o => o).ToJsonResultAsync();
    }
}
