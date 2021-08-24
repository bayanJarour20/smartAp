using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.QuestionDto;
using SmartStart.Repository.Main.QuestionServices;
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
    public class QuestionController : ElControllerBase<Guid, IQuestionRepository>
    {
        public QuestionController(IQuestionRepository repository) : base(repository) { }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? year, [FromQuery] Guid? semesterId, [FromQuery] Guid? subjectId) => await repository.GetAll(year, semesterId, subjectId).ToJsonResultAsync();

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] QuestionTagsAnswersExamsDto question) => await repository.Add(question).ToJsonResultAsync();

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] QuestionTagsAnswersExamsDto question) => await repository.Update(question).ToJsonResultAsync();

        [HttpGet("{id}")]
        public async Task<IActionResult> Details([Required] Guid id) => await repository.Details(id).ToJsonResultAsync();

        [HttpPost]
        public async Task<IActionResult> Correct(QuestionAnswersDto question) => await repository.Correct(question).ToJsonResultAsync();

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([Required] Guid id) => await repository.Delete(id).IntoAsync(o => o).ToJsonResultAsync();
    }
}
