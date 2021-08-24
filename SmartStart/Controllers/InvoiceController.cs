using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.InvoiceDto;
using SmartStart.Repository.Invoice.InvoiceService;
using SmartStart.Security;
using SmartStart.SharedKernel.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ElAuthorizeDistributed(SmartStartRoles.Admin)]
    public class InvoiceController : ElControllerBase<Guid, IInvoiceRepository>
    {
        public InvoiceController(IInvoiceRepository repository) : base(repository) { }

        [HttpGet]
        public async Task<IActionResult> GetUsersDetails(Guid? posId) => await repository.GetUsersDetails(posId).ToJsonResultAsync();
        
        [HttpGet]
        public async Task<IActionResult> GetInvoicesByPosId(Guid posId) => await repository.GetInvoicesByPosId(posId).ToJsonResultAsync();
        
        [HttpGet]
        public async Task<IActionResult> FillInvoice(Guid posId) => await repository.FillInvoice(posId).ToJsonResultAsync();
        
        [HttpPost]
        public async Task<IActionResult> CreateInvoice(CreateInvoiceDto InvoiceDto) => await repository.CreateInvoice(InvoiceDto).ToJsonResultAsync();
        
        [HttpGet]
        public async Task<IActionResult> GetInvoiceById(Guid invoiceId) => await repository.GetInvoiceById(invoiceId).ToJsonResultAsync();
        
        [HttpDelete]
        public async Task<IActionResult> DeleteInvoice(Guid invoiceId) => await repository.DeleteInvoice(invoiceId).ToJsonResultAsync();

        [HttpDelete]
        public async Task<IActionResult> DeleteInvoiceRange(IEnumerable<Guid> invoiceIds) => await repository.DeleteInvoiceRange(invoiceIds).ToJsonResultAsync();
    }
}
