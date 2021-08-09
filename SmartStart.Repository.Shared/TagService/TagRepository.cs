using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext.General;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.TagDto;
using SmartStart.Model.Shared;
using SmartStart.SharedKernel.Enums;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Shared.TagService
{
    [ElRepository]
    class TagRepository : ElRepositoryGeneral<SmartStartDbContext, Guid, Tag, TagDto>, ITagRepository
    {
        public TagRepository(SmartStartDbContext context) : base(context) { }

        public async Task<OperationResult<IEnumerable<TagDto>>> Filter(TagTypes type)
        => await RepositoryHandler(_filter(type));

        private Func<OperationResult<IEnumerable<TagDto>>, Task<OperationResult<IEnumerable<TagDto>>>> _filter(TagTypes type)
        => async operation => {
            var list = await Query.Where(tag => tag.Type == type).Select(TagDto.Selector).ToListAsync();
            return operation.SetSuccess(list);
        };
    }
}
