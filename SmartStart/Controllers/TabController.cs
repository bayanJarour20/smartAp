using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.ExamDto;
using SmartStart.Repository.Main.ExamServices;
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
    public class TabController : ElControllerBase<Guid, IExamRepository>
    {
        public TabController(IExamRepository examRepository) : base(examRepository) { }

        #region Exam 
        [Route("api/Exam/GetAll")]
        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> GetAllExam()
            => await repository.GetAllExam().ToJsonResultAsync();

        [Route("api/Exam/Delete/{id}")]
        [HttpDelete, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> DeleteExam([Required] Guid id)
            => await repository.DeleteExam(id).IntoAsync(o => o).ToJsonResultAsync();

        [Route("api/Exam/MultiDelete/{ids}")]
        [HttpDelete, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> MultiDeleteExam([Required] IEnumerable<Guid> ids)
            => await repository.MultiDeleteExam(ids).IntoAsync(o => o).ToJsonResultAsync();

        [Route("api/Exam/Add")]
        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> AddExam(ExamDto dto)
            => await repository.AddExam(dto).ToJsonResultAsync();

        [Route("api/Exam/Update")]
        [HttpPut, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> UpdateExam(ExamDto dto)
            => await repository.UpdateExam(dto).ToJsonResultAsync();

        [Route("api/Exam/{id}/Question/GetAll")]
        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> GetAllExamQuestion([Required] Guid id)
            => await repository.GetAllExamQuestion(id).ToJsonResultAsync();
        #endregion

        #region - Bank -
        [Route("api/Bank/GetAll")]
        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> GetAllBank()
            => await repository.GetAllBank().ToJsonResultAsync();

        [Route("api/Bank/Delete/{id}")]
        [HttpDelete, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> DeleteBank([Required] Guid id)
            => await repository.DeleteBank(id).IntoAsync(o =>o).ToJsonResultAsync();

        [Route("api/Bank/MultiDelete/{ids}")]
        [HttpDelete, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> MultiDeleteBank([Required] IEnumerable<Guid> ids)
           => await repository.MultiDeleteBank(ids).IntoAsync(o => o).ToJsonResultAsync();

        [Route("api/Bank/Add")]
        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> AddBank(ExamDto dto)
            => await repository.AddBank(dto).ToJsonResultAsync();

        [Route("api/Bank/Update")]
        [HttpPut, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> UpdateBank(ExamDto dto)
            => await repository.UpdateBank(dto).ToJsonResultAsync();

        [Route("api/Bank/{id}/Question/GetAll")]
        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> GetAllBankQuestion([Required] Guid id)
            => await repository.GetAllBankQuestion(id).ToJsonResultAsync();
        #endregion

        #region - Interview -
        [Route("api/Interview/GetAll")]
        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> GetAllInterview()
            => await repository.GetAllInterview().ToJsonResultAsync();

        [Route("api/Interview/Delete/{id}")]
        [HttpDelete, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> DeleteInterview([Required] Guid id)
            => await repository.DeleteInterview(id).IntoAsync(o => o).ToJsonResultAsync();

        [Route("api/Interview/MultiDelete/{ids}")]
        [HttpDelete, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> MultiDelete([Required] IEnumerable<Guid> ids)
            => await repository.MultiDeleteInterview(ids).IntoAsync(o => o).ToJsonResultAsync();

        [Route("api/Interview/Add")]
        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> AddInterview(ExamDto dto)
            => await repository.AddInterview(dto).ToJsonResultAsync();

        [Route("api/Interview/Update")]
        [HttpPut, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> UpdateInterview(ExamDto dto)
            => await repository.UpdateInterview(dto).ToJsonResultAsync();

        [Route("api/Interview/{id}/Question/GetAll")]
        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> GetAllInterviewQuestion([Required] Guid id)
            => await repository.GetAllInterviewQuestion(id).ToJsonResultAsync();
        #endregion

        #region - Microscope -
        [Route("api/Microscope/GetAll")]
        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> GetAllMicroscope()
            => await repository.GetAllMicroscope().ToJsonResultAsync();

        [Route("api/Microscope/Details/{id}")]
        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> DetailsMicroscope([Required] Guid id)
            => await repository.DetailsMicroscope(id).ToJsonResultAsync();

        [Route("api/Microscope/Delete/{id}")]
        [HttpDelete, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> DeleteMicroscope([Required] Guid id)
            => await repository.DeleteMicroscope(id).ToJsonResultAsync();

        [Route("api/Microscope/MultiDelete/")]
        [HttpDelete, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> MultiDeleteMicroscope([Required] IEnumerable<Guid> ids)
            => await repository.MultiDeleteMicroscope(ids).ToJsonResultAsync();

        [Route("api/Microscope/Add")]
        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> AddMicroscope(ExamDto dto)
            => await repository.AddMicroscope(dto).ToJsonResultAsync();

        [Route("api/Microscope/Update")]
        [HttpPut, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> UpdateMicroscope(ExamDto dto)
            => await repository.UpdateMicroscope(dto).ToJsonResultAsync();

        [Route("api/Microscope/Sections/Update")]
        [HttpPut, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> UpdateSectionsMicroscope(SectionsMicroscopeDocumentsDto dto)
            => await repository.UpdateSectionsMicroscope(dto).ToJsonResultAsync();

        [Route("api/Microscope/Sections/Delete/{id}")]
        [HttpDelete, ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
        public async Task<IActionResult> DeleteSectionsMicroscope(Guid id)
            => await repository.DeleteSectionsMicroscope(id).ToJsonResultAsync();
        #endregion
    }
}
