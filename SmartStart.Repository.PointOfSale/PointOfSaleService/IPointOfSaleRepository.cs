using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using SmartStart.Repository.PointOfSale.DataTransferObject;
using SmartStart.Repository.PointOfSale.PointOfSaleService.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.PointOfSale.PointOfSaleService
{
    public interface IPointOfSaleRepository : IElRepository 
    {
        Task<OperationResult<PointOfSaleDto>> Fetch(Guid id);
        Task<OperationResult<IEnumerable<PackageDto>>> GetPackages(Guid UserId);
        Task<OperationResult<CodePackagesDto>> GenerateCode(Guid id, GenerateCodeDto generateCode);
        Task<OperationResult<NotificationCollectionDto>> GetNotifications();

        //Mobile
        Task<OperationResult<object>> GetPointOfSales();


        Task<OperationResult<PointOfSaleAccountDto>> Create(PointOfSaleAccountDto account);
        Task<OperationResult<PointOfSaleAccountDto>> Update(PointOfSaleAccountDto account);
        Task<OperationResult<bool>> Delete(Guid id);
        Task<OperationResult<bool>> MultiDelete(List<Guid> ids);
        Task<OperationResult<bool>> Block(Guid id);
        Task<OperationResult<IEnumerable<PointOfSaleAccountDto>>> GetAll();
        Task<OperationResult<PointOfSaleAccountCodesDto>> Details(Guid id);
    }
}
