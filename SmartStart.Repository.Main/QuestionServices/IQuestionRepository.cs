using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using SmartStart.DataTransferObject.QuestionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Main.QuestionServices
{
    public interface IQuestionRepository : IElRepository
    {
        Task<OperationResult<IEnumerable<QuestionTagsAnswersDto>>> GetAll(int? year, Guid? semesterId, Guid? subjectId);
        Task<OperationResult<QuestionTagsAnswersExamsDto>> Add(QuestionTagsAnswersExamsDto question);
        Task<OperationResult<QuestionTagsAnswersExamsDto>> Update(QuestionTagsAnswersExamsDto question);
        Task<OperationResult<QuestionTagsAnswersExamsDto>> Details(Guid id);
        Task<OperationResult<bool>> Delete(Guid id);
        Task<OperationResult<QuestionAnswersDto>> Correct(QuestionAnswersDto question);
    }
}
