using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartStart.Model.Business;
using SmartStart.Model.Main;
using SmartStart.Model.Security;
using SmartStart.Model.Shared;
using SmartStart.SharedKernel.Enums;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Shared.HomeService
{
    [ElRepository]
    public class HomeRepository : ElRepository<SmartStartDbContext, Guid>, IHomeRepository
    {

        public HomeRepository(SmartStartDbContext context) : base(context) { }

        public async Task<OperationResult<object>> Home()
         => await RepositoryHandler(_home);

        private Func<OperationResult<object>, Task<OperationResult<object>>> _home
            => async operation => {

                var dateNow = DateTime.Now.ToLocalTime();
                var dateAfterMonth = dateNow.AddMonths(-1);
                
                // normal counts
                var subjectCount = await _query<Subject>().LongCountAsync();
                var tagCount = await _query<Tag>().LongCountAsync();
                var examCounts = await _query<Exam>().GroupBy(g => 1)
                                                     .Select(g => new
                                                     {
                                                         examCount = g.LongCount(x => x.Type == TabTypes.Exam),
                                                         bankCount = g.LongCount(x => x.Type == TabTypes.Bank),
                                                         interviewCount = g.LongCount(x => x.Type == TabTypes.Interview),
                                                         microscopeCount = g.LongCount(x => x.Type == TabTypes.Microscope),
                                                     }).FirstOrDefaultAsync();

                // code cards
                var codes = await _query<Code>().Where(code => code.DateCreated.Year == dateNow.Year
                                                            && (code.DateCreated.Month == dateNow.Month
                                                            || (code.DateCreated.Month != 1
                                                            && code.DateCreated.Month == dateAfterMonth.Month))).GroupBy(g => 1)
                                                .Select(g => new
                                                {
                                                    TotalValueCode = g.Where(x => x.DateCreated.Month == dateNow.Month).Sum(x => x.Value),
                                                    TotalCodeCount = g.LongCount(x => x.DateCreated.Month == dateNow.Month),
                                                    worthlyValueCode = g.Where(x => !x.InvoiceId.HasValue && x.DateCreated.Month == dateNow.Month).Sum(x => x.Value),
                                                    worthlyCodeCount = g.LongCount(x => !x.InvoiceId.HasValue && x.DateCreated.Month == dateNow.Month),
                                                    LastTotalValueCode = g.Where(x => x.DateCreated.Month == dateNow.Month).Sum(x => x.Value),
                                                    LastworthlyValueCode = g.Where(x => !x.InvoiceId.HasValue && x.DateCreated.Month == dateAfterMonth.Month).Sum(x => x.Value),
                                                }).FirstOrDefaultAsync();

                // user cards
                var allapp = (await _query<AppUser>().Where(user => user.DateCreated.Year == dateNow.Year
                                                                 && (user.Type == UserTypes.User
                                                                 || user.Type == UserTypes.Seller))
                                                     .Select(user => new { user.DateCreated.Month, user.Type }).ToListAsync())
                                                     .ToLookup(x => (x.Type), x => x.Month);

                var users = allapp[UserTypes.User];
                var usersMonthly = System.Linq.Enumerable.Range(1, 12)
                                                         .Select(itr => new
                                                         {
                                                             Month = itr,
                                                             Count = users.LongCount(x => x == itr),
                                                         });
                var sellers = allapp[UserTypes.Seller];
                var sellersMonthly = System.Linq.Enumerable.Range(1, 12)
                                                           .Select(itr => new
                                                           {
                                                               Month = itr,
                                                               Count = sellers.LongCount(x => x == itr),
                                                           });
                var lastFiveYear = dateNow.AddYears(-5).Year;

                // invoice card
                var _invoices = (await _query<Invoice>().Where(invoice => invoice.Date.Year > lastFiveYear)
                                                        .Select(x => new {
                                                            x.Date.Year,
                                                            x.Date.Month,
                                                            x.Total
                                                        }).ToListAsync()).ToLookup(x => (x.Year, x.Month), x => x.Total);

                var invoices = System.Linq.Enumerable.Range(lastFiveYear + 1, 5).Select(yearIter => new
                {
                    Year = yearIter,
                    Data = System.Linq.Enumerable.Range(1, 12).Select(monthIter => new
                    {

                        Month = monthIter,
                        Total = _invoices[(yearIter, monthIter)].Sum(),
                    })
                });

                return operation.SetSuccess(new
                {
                    subjectCount,
                    tagCount,
                    examCounts.examCount,
                    examCounts.interviewCount,
                    examCounts.bankCount,
                    examCounts.microscopeCount,

                    codes.TotalValueCode,
                    codes.TotalCodeCount,
                    codes.worthlyValueCode,
                    codes.worthlyCodeCount,
                    codes.LastTotalValueCode,
                    codes.LastworthlyValueCode,

                    usersMonthly,
                    sellersMonthly,

                    invoices
                });
            };
    }
}
