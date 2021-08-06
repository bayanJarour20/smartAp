using Elkood.Web.Service.BoundedContext.General;
using Elkood.Web.Service.DependencyInjection;
using SmartStart.DataTransferObject.FeedbackDto;
using SmartStart.Model.Setting;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Setting.FeedbackService
{
    [ElRepository]
    public class FeedbackRepository : ElRepositoryGeneral<SmartStartDbContext, Guid, Feedback, FeedbackDto>, IFeedbackRepository
    {
        public FeedbackRepository(SmartStartDbContext context) : base(context) { }
    }
}
