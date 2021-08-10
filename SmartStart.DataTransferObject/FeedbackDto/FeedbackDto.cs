using Elkood.Web.Common.AuxiliaryImplemental.Interfaces;
using SmartStart.Model.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.FeedbackDto
{
    public class FeedbackDto : ISelector<Feedback, FeedbackDto>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Reply { get; set; }
        public DateTime? ReplyDate { get; set; }
        public Guid AppUserId { get; set; }

        public static Expression<Func<Feedback, FeedbackDto>> Selector { get; set; } = feedback => new FeedbackDto() { Id = feedback.Id, Title = feedback.Title, Body = feedback.Body, Reply = feedback.Reply, ReplyDate = feedback.ReplyDate, AppUserId = feedback.AppUserId };
        public static Expression<Func<FeedbackDto, Feedback>> InverseSelector { get; set; } = feedback => new Feedback() { Id = feedback.Id, Title = feedback.Title, Body = feedback.Body, Reply = feedback.Reply, ReplyDate = feedback.ReplyDate, AppUserId = feedback.AppUserId };
        public static Action<FeedbackDto, Feedback> AssignSelector { get; set; } = (dto, entity) => { entity.Title = dto.Title; entity.Body = dto.Body; };
    }
}
