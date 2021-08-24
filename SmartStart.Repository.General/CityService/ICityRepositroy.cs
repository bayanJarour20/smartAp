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
    public interface ICityRepository : IElRepositoryGeneral<Guid, City, CityDto> { 
        Task<OperationResult<bool>> DeleteRange(IEnumerable<Guid> ids);
    }
}
