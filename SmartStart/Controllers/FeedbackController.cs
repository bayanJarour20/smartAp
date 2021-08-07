using Elkood.Web.MVC;
using Elkood.Web.MVC.Security.Attribute;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.FeedbackDto;
using SmartStart.Repository.Setting.FeedbackService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FeedbackController : ElControllerBaseGeneral<Guid, IFeedbackRepository, FeedbackDto>
    {
        public FeedbackController(IFeedbackRepository repository) : base(repository) { }

        [ElAuthorize]
        public override Task<IActionResult> Add(FeedbackDto dto)
        {
            dto.AppUserId = Key.Value;
            return base.Add(dto);
        }
    }
}
