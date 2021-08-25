using Elkood.Web.Common.AuxiliaryImplemental.Interfaces;
using SmartStart.Model.Setting;
using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.AdvertisementDto
{
    public class AdvertisementDto :ISelector<Advertisement, AdvertisementDto>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AdvertisementTypes Type { get; set; }

        public static Expression<Func<Advertisement, AdvertisementDto>> Selector { get; set; } = advertisement => new AdvertisementDto() { Id = advertisement.Id, Title = advertisement.Title, ImagePath = advertisement.ImagePath, StartDate = advertisement.StartDate, EndDate = advertisement.EndDate, Type = advertisement.Type, };
        public static Expression<Func<AdvertisementDto, Advertisement>> InverseSelector { get; set; } = advertisement => new Advertisement() { Id = advertisement.Id, Title = advertisement.Title, ImagePath = advertisement.ImagePath, StartDate = advertisement.StartDate, EndDate = advertisement.EndDate, Type = advertisement.Type, };
        public static Action<AdvertisementDto, Advertisement> AssignSelector { get; set; } = (dto, entity) => { entity.Title = dto.Title; entity.ImagePath = dto.ImagePath; entity.StartDate = dto.StartDate; entity.EndDate = dto.EndDate; entity.Type = dto.Type; };
    }
}
