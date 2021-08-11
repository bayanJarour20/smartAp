using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.SubjectDto;
using SmartStart.Repository.Main.SubjectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SubjectController : ElControllerBase<Guid, ISubjectRepository>
    {
        public SubjectController(ISubjectRepository repository) : base(repository) { }

        [HttpGet]
        public async Task<IActionResult> GetAll(int? year, Guid? semesterId, Guid? facultyId)
            => await repository.GetAll(year, semesterId, facultyId).ToJsonResultAsync();

        [HttpPost]
        public async Task<IActionResult> SetSubject([FromForm] SubjectDetailsDto subjectDto)
            => await repository.SetSubject(subjectDto, subjectDto.File).ToJsonResultAsync();

        [HttpGet]
        public async Task<IActionResult> SubjectDetails(Guid subjectId)
            => await repository.SubjectDetails(subjectId).ToJsonResultAsync();
        
        [HttpDelete]
        public async Task<IActionResult> RemoveSubject(Guid subjectId)
           => await repository.RemoveSubject(subjectId).ToJsonResultAsync();


    }
}
