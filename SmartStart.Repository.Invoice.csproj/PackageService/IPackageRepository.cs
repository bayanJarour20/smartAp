using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using SmartStart.DataTransferObject.PackageDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Invoice.PackageService
{
    public interface IPackageRepository : IElRepository
    {
        Task<OperationResult<IEnumerable<PackageDetailsDto>>> GetAll();
        Task<OperationResult<PackageSubjectFilterDto>> Details(Guid id);
        Task<OperationResult<PackageDto>> Add(PackageSubjectDto dto);
        Task<OperationResult<PackageDto>> Update(PackageSubjectDto dto);
        Task<OperationResult<bool>> RemovePackage(Guid Id);
        Task<OperationResult<bool>> RemovePackages(List<Guid> Ids);

        Task<OperationResult<bool>> Init();
    }
}
