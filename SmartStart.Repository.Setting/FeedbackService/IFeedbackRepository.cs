using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext.General;
using SmartStart.DataTransferObject.FeedbackDto;
using SmartStart.Model.Setting;

namespace SmartStart.Repository.Setting.FeedbackService
{
    public interface IFeedbackRepository : IElRepositoryGeneral<Guid, Feedback, FeedbackDto>
    {
        Task<OperationResult<IEnumerable<FeedbackDetailsDto>>> GetAll();
        Task<OperationResult<FeedbackDetailsDto>> Details(Guid id);
        Task<OperationResult<bool>> DeleteRange(IEnumerable<Guid> ids);
    }
}
