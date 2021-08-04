using Elkood.Web.Common.AuxiliaryImplemental.Interfaces;
using SmartStart.Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.CityDto
{
    public class CityDto : ISelector<City, CityDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public static Expression<Func<City, CityDto>> Selector { get; set; } = city => new CityDto() { Id = city.Id, Name = city.Name };
        public static Expression<Func<CityDto, City>> InverseSelector { get; set; } = city => new City() { Id = city.Id, Name = city.Name };
        public static Action<CityDto, City> AssignSelector { get; set; } = (dto, entity) => { entity.Name = dto.Name; };
    }
}
