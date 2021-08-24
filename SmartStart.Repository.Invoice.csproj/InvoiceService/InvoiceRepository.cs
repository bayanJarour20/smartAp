using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.InvoiceDto;
using SmartStart.Model.Business;
using SmartStart.Model.Security;
using SmartStart.SharedKernel.Enums;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Invoice.InvoiceService
{
    [ElRepository]
    public class InvoiceRepository : ElRepository<SmartStartDbContext, Guid, Model.Business.Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(SmartStartDbContext context) : base(context) { }

        public async Task<OperationResult<IEnumerable<UserInvoiceDto>>> GetUsersDetails(Guid? posId)
        => await RepositoryHandler(_getUsersDetails(posId));
        public async Task<OperationResult<InvoiceDetailsDto>> GetInvoicesByPosId(Guid posId)
        => await RepositoryHandler(_getInvoicesByPosId(posId));
        public async Task<OperationResult<CreateInvoiceDto>> FillInvoice(Guid posId)
        => await RepositoryHandler(_fillInvoice(posId));
        public async Task<OperationResult<CreateInvoiceDto>> CreateInvoice(CreateInvoiceDto InvoiceDto)
        => await RepositoryHandler(_createInvoice(InvoiceDto));
        public async Task<OperationResult<CreateInvoiceDto>> GetInvoiceById(Guid invoiceId)
        => await RepositoryHandler(_getInvoiceById(invoiceId));
        public async Task<OperationResult<bool>> DeleteInvoice(Guid invoiceId)
        => await RepositoryHandler(_deleteInvoice(invoiceId));

        public async Task<OperationResult<bool>> DeleteInvoiceRange(IEnumerable<Guid> ids)
       => await RepositoryHandler(_deleteInvoiceRange(ids));


        private Func<OperationResult<IEnumerable<UserInvoiceDto>>, Task<OperationResult<IEnumerable<UserInvoiceDto>>>> _getUsersDetails(Guid? posId)
        => async operation => {

            var res = (await _query<AppUser>().Where(user => !user.DateDeleted.HasValue
                                                         && user.Type == UserTypes.Seller
                                                         && (posId == null || posId == user.Id))
                                             .Select(user => new
                                             {
                                                 PosId = user.Id,
                                                 posName = user.Name,
                                                 CodeCount = user.Codes.Count(),
                                                 LastMatchDate = user.Invoices.Max(invoice => (DateTime?)invoice.Date),
                                                 PaidMoney = user.Invoices.Sum(invoice => invoice.Total),
                                                 BalanceDue = user.Codes.Where(code => code.UserId.HasValue
                                                                                    && !code.InvoiceId.HasValue)
                                                                        .Select(code => code.Value * (1 - code.DiscountRate)
                                                                                                   * (1 - user.Rates.Where(rate => code.DateActivated >= rate.DateCreated
                                                                                                                                && (rate.DateUpdated == null || code.DateActivated <= rate.DateUpdated))
                                                                                                                    .Select(x => x.DiscountRate).SingleOrDefault()))
                                             }).ToListAsync())
                                             .Select(user => new UserInvoiceDto
                                             {
                                                 PosId = user.PosId,
                                                 PosName = user.posName,
                                                 CodeCount = user.CodeCount,
                                                 LastMatchDate = user.LastMatchDate,
                                                 BalanceDue = user.BalanceDue.Sum(),
                                                 PaidMoney = user.PaidMoney,
                                             });
            return operation.SetSuccess(res);
        };
        private Func<OperationResult<InvoiceDetailsDto>, Task<OperationResult<InvoiceDetailsDto>>> _getInvoicesByPosId(Guid posId)
        => async operation => {

            var list = await _query<AppUser>().Where(x => x.Id == posId).Select(user => new InvoiceDetailsDto()
            {
                Name = user.Name,
                Invoices = user.Invoices.Select(invoice => new InvoiceDto
                {
                    InvoiceId = invoice.Id,
                    Date = invoice.Date,
                    InvoiceNumber = invoice.SerialNumber,
                    CodeCount = invoice.Codes.Count(),
                    Total = invoice.Total
                }),
            }

            )
            .FirstOrDefaultAsync();
            return operation.SetSuccess(list);
        };
        private Func<OperationResult<CreateInvoiceDto>, Task<OperationResult<CreateInvoiceDto>>> _fillInvoice(Guid posId)
        => async operation => {
            var invoiceNumber = "";
            if (!Query.Any())
            {
                invoiceNumber = "1";
            }
            else
            {
                var temp = await Query.MaxAsync(invoice => invoice.SerialNumber);
                invoiceNumber = (Int32.Parse(temp) + 1).ToString();
            }
            var user = _query<AppUser>().Include(user => user.Codes)
                                        .Include(user => user.Rates)
                                        .SingleOrDefault(user => !user.DateDeleted.HasValue
                                                              && user.Id == posId);
            var codeList = user.Codes.Where(code => !code.InvoiceId.HasValue
                                                 && code.UserId.HasValue);
            var rateList = user.Rates;
            var InvoiceDetails = getInvoiceDetails(codeList.ToList(), rateList.ToList());
            var res = await _query<AppUser>().Where(user => !user.DateDeleted.HasValue
                                                         && user.Id == posId)
                                             .Select(user => new CreateInvoiceDto
                                             {
                                                 PosId = posId,
                                                 PosName = user.Name,
                                                 InvoiceNumber = invoiceNumber,
                                                 Date = DateTime.Now,
                                                 CodeCount = InvoiceDetails.Item1,
                                                 ActualCost = InvoiceDetails.Item2,
                                                 DueCompany = InvoiceDetails.Item3,
                                                 PaidMoney = InvoiceDetails.Item3,
                                                 Notes = "",
                                                 Codes = InvoiceDetails.Item4
                                             }).SingleOrDefaultAsync();
            return operation.SetSuccess(res);
        };
        private Func<OperationResult<CreateInvoiceDto>, Task<OperationResult<CreateInvoiceDto>>> _createInvoice(CreateInvoiceDto InvoiceDto)
        => async operation => {
            var Invoice = new Model.Business.Invoice()
            {
                DateCreated = DateTime.Now,
                Date = InvoiceDto.Date,
                Note = InvoiceDto.Notes,
                Id = Guid.NewGuid(),
                SellerId = InvoiceDto.PosId,
                SerialNumber = InvoiceDto.InvoiceNumber,
                Total = InvoiceDto.PaidMoney
            };
            foreach (var code in InvoiceDto.Codes)
            {
                var _code = await _trackingQuery<Code>().Where(c => c.Id == code.CodeId).SingleOrDefaultAsync();
                _code.InvoiceId = Invoice.Id;
                _code.DateUpdated = DateTime.Now;
                Context.Update(_code);
            }
            await Context.AddAsync(Invoice);
            await Context.SaveChangesAsync();
            InvoiceDto.InvoiceId = Invoice.Id;
            return operation.SetSuccess(InvoiceDto);
        };
        private Func<OperationResult<CreateInvoiceDto>, Task<OperationResult<CreateInvoiceDto>>> _getInvoiceById(Guid invoiceId)
        => async operation => {
            var invoice = await Query.Include(invoice => invoice.Codes)
                                     .Include(invoice => invoice.Seller)
                                     .ThenInclude(seller => seller.Rates)
                                     .Where(invoice => invoice.Id == invoiceId)
                                     .SingleOrDefaultAsync();
            if (invoice == null)
            {
                return operation.SetFailed("Invoice Not Found", OperationResultTypes.Forbidden);
            }

            var codeList = invoice.Codes;
            var rateList = invoice.Seller.Rates;
            var InvoiceDetails = getInvoiceDetails(codeList.ToList(), rateList.ToList());
            var res = await Query.Where(invoice => invoice.Id == invoiceId)
                                 .Select(invoice => new CreateInvoiceDto
                                 {
                                     CodeCount = InvoiceDetails.Item1,
                                     ActualCost = InvoiceDetails.Item2,
                                     DueCompany = InvoiceDetails.Item3,
                                     Date = invoice.Date,
                                     InvoiceId = invoice.Id,
                                     Notes = invoice.Note,
                                     PaidMoney = invoice.Total,
                                     InvoiceNumber = invoice.SerialNumber,
                                     PosId = invoice.SellerId,
                                     PosName = invoice.Seller.Name,
                                     Codes = InvoiceDetails.Item4
                                 }).SingleOrDefaultAsync();
            return operation.SetSuccess(res);
        };
        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _deleteInvoice(Guid invoiceId)
        => async operation => {
            var invoice = await TrackingQuery.Include(inv => inv.Codes)
                                             .Where(invoice => invoice.Id == invoiceId)
                                             .SingleOrDefaultAsync();
            if (invoice == null)
            {
                return operation.SetFailed("Invoice Not Found", OperationResultTypes.NotExist);
            }
            var temp = TrackingQuery.OrderBy(t => t.Date).LastOrDefault();
            if (temp == null || temp.Id != invoiceId)
            {
                return operation.SetFailed("Only Last Invoice Can Be Deleted");
            }
            foreach (var code in invoice.Codes)
            {
                code.DateUpdated = DateTime.Now;
                code.InvoiceId = null;
            }
            Context.UpdateRange(invoice.Codes);
            Context.SoftDelete(invoice);
            await Context.SaveChangesAsync();
            return operation.SetSuccess(true);
        };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _deleteInvoiceRange(IEnumerable<Guid> ids)
        => async operation => {


            var InvoiceModels = await TrackingQuery.Include(inv => inv.Codes)
                                             .Where(invoice => ids.Contains(invoice.Id))
                                             .ToListAsync();
            if (InvoiceModels.Count == null)
            {
                return operation.SetFailed("Invoices Not Found", OperationResultTypes.Forbidden);
            }

            var temp = TrackingQuery.OrderBy(t => t.Date).LastOrDefault();

            //if (temp == null || temp.Id != invoiceId)
            //{
            //    return operation.SetFailed("Only Last Invoice Can Be Deleted");
            //}

            //foreach (var code in invoice.Codes)
            //{
            //    code.DateUpdated = DateTime.Now;
            //    code.InvoiceId = null;
            //}

            //Context.UpdateRange(invoice.Codes);

            foreach (SmartStart.Model.Business.Invoice invoice in InvoiceModels)
            {
                Context.SoftDelete(invoice);
            }

            
            await Context.SaveChangesAsync();
            return operation.SetSuccess(true);
        };

        #region Helper Functions 
        private Tuple<int, double, double, List<CodeInvoiceDto>> getInvoiceDetails(List<Code> codes, List<Rate> rates)
        {
            int codeCount = codes.Count();
            double actualCost = 0;
            double dueCompany = 0;
            List<CodeInvoiceDto> res = new List<CodeInvoiceDto>();
            foreach (var code in codes)
            {
                var temp = code.Value * (1 - code.DiscountRate);
                actualCost += temp;
                dueCompany += temp * (1 - rates.Where(rate => code.DateActivated >= rate.DateCreated
                                                                     && (rate.DateUpdated == null || code.DateActivated <= rate.DateUpdated))
                    .Select(x => x.DiscountRate).SingleOrDefault());

                res.Add(new CodeInvoiceDto
                {
                    CodeId = code.Id,
                    Code = code.Hash,
                    Date = code.DateActivated,
                    DiscountRate = code.DiscountRate,
                    Value = code.Value,
                    PosRate = rates.Where(rate => code.DateActivated >= rate.DateCreated
                                                                      && (rate.DateUpdated == null || code.DateActivated <= rate.DateUpdated))
                                .Select(x => x.DiscountRate).SingleOrDefault()
                });
            }
            return new Tuple<int, double, double, List<CodeInvoiceDto>>(codeCount, actualCost, dueCompany, res);
        }
        #endregion
    }
}
