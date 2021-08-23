using Elkood.Web.Common.AuxiliaryImplemental.Interfaces;
using SmartStart.Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.UniverstiyDto
{
    public class UniversityDto : ISelector<University, UniversityDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CityId { get; set; }
        public string CityName { get; set; }

        public static Expression<Func<University, UniversityDto>> Selector { get; set; } = university => new UniversityDto() { Id = university.Id, Name = university.Name, CityId = university.CityId };
        public static Expression<Func<UniversityDto, University>> InverseSelector { get; set; } = university => new University() { Id = university.Id, Name = university.Name, CityId = university.CityId };
        public static Action<UniversityDto, University> AssignSelector { get; set; } = (dto, entity) => { entity.Name = dto.Name; };

    }
}
