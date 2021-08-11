using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Elkood.Web.MVC.Security.Attribute;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.FeedbackDto;
using SmartStart.Repository.Setting.FeedbackService;
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
    public class FeedbackController : ElControllerBaseGeneral<Guid, IFeedbackRepository, FeedbackDto>
    {
        private readonly IFeedbackRepository feedbackRepository;

        public FeedbackController(IFeedbackRepository feedbackRepository) : base(feedbackRepository)
        {
            this.feedbackRepository = feedbackRepository;
        }

        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public override Task<IActionResult> Fetch() => base.Fetch();
        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public override Task<IActionResult> GetById(Guid id) => base.GetById(id);
        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public override Task<IActionResult> Modify(FeedbackDto dto) => base.Modify(dto);
        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public override Task<IActionResult> Update(FeedbackDto dto) => base.Update(dto);
        [HttpDelete, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public override Task<IActionResult> Delete(Guid id) => base.Delete(id);

        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.User)]
        public override Task<IActionResult> Add(FeedbackDto dto)
        {
            dto.AppUserId = Key.Value;
            return base.Add(dto);
        }

        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> GetAll() => await feedbackRepository.GetAll().ToJsonResultAsync();

        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> Details([Required] Guid id) => await feedbackRepository.Details(id).ToJsonResultAsync();

        [HttpDelete, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> MultiDelete([Required] IEnumerable<Guid> ids) => await feedbackRepository.MultiDelete(ids).ToJsonResultAsync();
    }
}
