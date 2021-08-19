using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using SmartStart.DataTransferObject.ExamDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Main.ExamServices
{
    public interface IExamRepository : IElRepository
    {

        Task<OperationResult<IEnumerable<ExamDetailsDto>>> GetAllExam();
        Task<OperationResult<bool>> DeleteExam(Guid id);
        Task<OperationResult<bool>> MultiDeleteExam(IEnumerable<Guid> ids);
        Task<OperationResult<ExamDetailsDto>> AddExam(ExamDto dto);
        Task<OperationResult<ExamDetailsDto>> UpdateExam(ExamDto dto);
        Task<OperationResult<IEnumerable<ExamDetailsQuestionDto>>> GetAllExamQuestion(Guid id);

        Task<OperationResult<IEnumerable<ExamDetailsDto>>> GetAllBank();
        Task<OperationResult<bool>> DeleteBank(Guid id);
        Task<OperationResult<bool>> MultiDeleteBank(IEnumerable<Guid> ids);
        Task<OperationResult<ExamDetailsDto>> AddBank(ExamDto dto);
        Task<OperationResult<ExamDetailsDto>> UpdateBank(ExamDto dto);
        Task<OperationResult<IEnumerable<ExamDetailsQuestionDto>>> GetAllBankQuestion(Guid id);

        Task<OperationResult<IEnumerable<ExamDetailsDto>>> GetAllInterview();
        Task<OperationResult<bool>> DeleteInterview(Guid id);
        Task<OperationResult<bool>> MultiDeleteInterview(IEnumerable<Guid> ids);
        Task<OperationResult<ExamDetailsDto>> AddInterview(ExamDto dto);
        Task<OperationResult<ExamDetailsDto>> UpdateInterview(ExamDto dto);
        Task<OperationResult<IEnumerable<ExamDetailsQuestionDto>>> GetAllInterviewQuestion(Guid id);

    }
}
