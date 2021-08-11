using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartStart.Repository.PointOfSale.PointOfSaleService;
using SmartStart.Repository.PointOfSale.PointOfSaleService.DataTransferObject;
using SmartStart.Security;
using SmartStart.SharedKernel.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PointOfSaleController : ElControllerBase<Guid, IPointOfSaleRepository>
    {
        public PointOfSaleController(IPointOfSaleRepository pointOfSaleRepository) : base(pointOfSaleRepository) { }

        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Seller)]
        public async Task<IActionResult> Fetch() => await repository.Fetch(Key.Value).ToJsonResultAsync();

        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Seller)]
        public async Task<IActionResult> GetPackages() => await repository.GetPackages(Key.Value).ToJsonResultAsync();

        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Seller)]
        public async Task<IActionResult> GetNotifications() => await repository.GetNotifications().ToJsonResultAsync();

        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.Seller)]
        public async Task<IActionResult> GenerateCode(GenerateCodeDto generateCode) => await repository.GenerateCode(Key.Value, generateCode).ToJsonResultAsync();

        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.User)]
        public async Task<IActionResult> GetPointOfSales() => await repository.GetPointOfSales().ToJsonResultAsync();





        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> Create(PointOfSaleAccountDto account) => await repository.Create(account).ToJsonResultAsync();

        [HttpPut, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> Update(PointOfSaleAccountDto account) => await repository.Update(account).ToJsonResultAsync();

        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> GetAll() => await repository.GetAll().ToJsonResultAsync();

        [HttpDelete, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> Delete([Required] Guid id) => await repository.Delete(id).IntoAsync(o => o).ToJsonResultAsync();

        [HttpPut("{id}"), ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> Block(Guid id) => await repository.Block(id).IntoAsync(o => o).ToJsonResultAsync();

        [HttpGet("{id}"), ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> Details(Guid id) => await repository.Details(id).ToJsonResultAsync();

    }
}
