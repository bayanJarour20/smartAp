﻿using Elkood.Web.Common.ContextResult.OperationContext;
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
    }
}
