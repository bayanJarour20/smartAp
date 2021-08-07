using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext.General;
using Microsoft.AspNetCore.Http;
using SmartStart.DataTransferObject.AdvertisementDto;
using SmartStart.Model.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Setting.AdvertisementService
{
    public interface IAdvertisementRepository : IElRepositoryGeneral<Guid, Advertisement, AdvertisementDto>
    {
        Task<OperationResult<AdvertisementDto>> Upload(AdvertisementDto dto, IFormFile file);
        Task<OperationResult<IEnumerable<AdvertisementDto>>> GetAdvertisement();
    }
}
