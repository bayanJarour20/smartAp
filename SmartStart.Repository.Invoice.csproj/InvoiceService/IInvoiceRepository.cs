using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using SmartStart.DataTransferObject.InvoiceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Invoice.InvoiceService
{
    public interface IInvoiceRepository : IElRepository
    {
        public Task<OperationResult<IEnumerable<UserInvoiceDto>>> GetUsersDetails(Guid? posId);
        public Task<OperationResult<InvoiceDetailsDto>> GetInvoicesByPosId(Guid posId);
        public Task<OperationResult<CreateInvoiceDto>> FillInvoice(Guid posId);
        public Task<OperationResult<CreateInvoiceDto>> CreateInvoice(CreateInvoiceDto InvoiceDto);
        public Task<OperationResult<CreateInvoiceDto>> GetInvoiceById(Guid invoiceId);
        public Task<OperationResult<bool>> DeleteInvoice(Guid invoiceId);
        public Task<OperationResult<bool>> DeleteInvoiceRange(IEnumerable<Guid> ids);
    }
}
