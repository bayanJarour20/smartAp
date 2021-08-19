using Elkood.Web.Service.BoundedContext.Store;
using SmartStart.DataTransferObject.FeedbackDto;
using SmartStart.Model.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static SmartStart.Repository.Setting.FeedbackService.QueryStore.FeedbackStore;

namespace SmartStart.Repository.Setting.FeedbackService.QueryStore
{
    [ElStore]
    public class FeedbackStore : ElStore<_Query, _Filter>
    {
        public class _Query : _IQuery
        {
            public Expression<Func<Feedback, FeedbackDetailsDto>> SelectorDetails = feedback => new FeedbackDetailsDto()
            {
                Id = feedback.Id,
                AppUserId = feedback.AppUserId,
                AppUserName = feedback.AppUser.Name,
                Title = feedback.Title,
                Body = feedback.Body,
                Reply = feedback.Reply,
                ReplyDate = feedback.ReplyDate,
                SendDate = feedback.DateCreated,
            };
        }

        public class _Filter : _IFilter
        {
            public Expression<Func<Feedback, bool>> FK(Guid id) => feedback => feedback.Id == id;
        }
    }
}
 