using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.FeedbackDto;
using SmartStart.Model.Setting;
using SmartStart.Repository.Setting.FeedbackService.QueryStore;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Setting.FeedbackService
{
    [ElRepository]
    public class FeedbackRepository : ElRepository<SmartStartDbContext, Guid, Feedback, FeedbackStore, FeedbackDto>, IFeedbackRepository
    {
        public FeedbackRepository(SmartStartDbContext context) : base(context) { }

       

        public async Task<OperationResult<IEnumerable<FeedbackDetailsDto>>> GetAll()
            => await RepositoryHandler(_getAll());

        

        public async Task<OperationResult<FeedbackDetailsDto>> Details(Guid id)
            => await RepositoryHandler(_details(id));

        
        private Func<OperationResult<IEnumerable<FeedbackDetailsDto>>, Task<OperationResult<IEnumerable<FeedbackDetailsDto>>>> _getAll()
            => async operation => operation.SetSuccess(await Query.Select(Store.Query.SelectorDetails).ToListAsync());

        private Func<OperationResult<FeedbackDetailsDto>, Task<OperationResult<FeedbackDetailsDto>>> _details(Guid id)
            => async operation => 
            (await Query.Where(Store.Filter.FK(id)).Select(Store.Query.SelectorDetails).FirstOrDefaultAsync()).ToOperationResult();


    }
}
