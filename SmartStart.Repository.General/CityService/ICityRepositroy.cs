using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext.General;
using SmartStart.DataTransferObject.CityDto;
using SmartStart.Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.General.CityService
{
    public interface ICityRepositroy : IElRepositoryGeneral<Guid, City, CityDto>
    {
        Task<OperationResult<IEnumerable<CityDto>>> GetCities();

        Task<OperationResult<CityDto>> Add(CityDto cityDto);

        Task<OperationResult<CityDto>> Update(CityDto cityDto);

        Task<OperationResult<bool>> Delete(Guid id);

        Task<OperationResult<bool>> DeleteRange(IEnumerable<Guid> ids);
    }
}
