using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.TagDto;
using SmartStart.Repository.Shared.TagService;
using SmartStart.Security;
using SmartStart.SharedKernel.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ElAuthorizeDistributed(SmartStartRoles.Admin, SmartStartRoles.Entry)]
    public class TagController : ElControllerBaseGeneral<Guid, ITagRepository, TagDto>
    {
        public TagController(ITagRepository repository) : base(repository) { }

        [HttpDelete]
        public async Task<IActionResult> RemoveTags(List<Guid> tagIds)
            => await repository.RemoveTags(tagIds).ToJsonResultAsync();
    }
}
