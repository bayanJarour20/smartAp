using Elkood.Web.Common.AuxiliaryImplemental.Interfaces;
using SmartStart.Model.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.TagDto
{
    public class TagDto : TagBaseDto, ISelector<Tag, TagDto>
    {
        public static Expression<Func<Tag, TagDto>> Selector { get; set; } = tag => new TagDto() { Id = tag.Id, Name = tag.Name, Type = tag.Type };
        public static Expression<Func<TagDto, Tag>> InverseSelector { get; set; } = tag => new Tag() { Id = tag.Id, Name = tag.Name, Type = tag.Type };
        public static Action<TagDto, Tag> AssignSelector { get; set; } = (dto, entity) => { entity.Name = dto.Name; };
    }
}
