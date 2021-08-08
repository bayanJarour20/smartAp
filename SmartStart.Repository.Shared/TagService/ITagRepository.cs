using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext.General;
using SmartStart.DataTransferObject.TagDto;
using SmartStart.Model.Shared;
using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Shared.TagService
{
    public interface ITagRepository : IElRepositoryGeneral<Guid, Tag, TagDto>
    {
        public Task<OperationResult<IEnumerable<TagDto>>> Filter(TagTypes type);

    }
}
