using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext.General;
using SmartStart.DataTransferObject.UniverstiyDto;
using SmartStart.Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.General.UniversityService
{
    public interface IUniversityRepository : IElRepositoryGeneral<Guid, University, UniversityDto>
    {
        Task<OperationResult<IEnumerable<UniversityDto>>> GetAll();

        Task<OperationResult<UniversityDto>> Add(UniversityDto universityDto);

        Task<OperationResult<UniversityDto>> Update(UniversityDto universityDto);

        Task<OperationResult<bool>> Delete(Guid id);

        Task<OperationResult<bool>> DeleteRange(IEnumerable<Guid> ids);
    }
}
