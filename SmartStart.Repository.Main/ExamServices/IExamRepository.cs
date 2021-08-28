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
        Task<OperationResult<BaseCollectionExamDto>> GetBasicExams();
        Task<OperationResult<IEnumerable<ExamDetailsDto>>> GetAllExam();
        Task<OperationResult<bool>> DeleteExam(Guid id);
        Task<OperationResult<bool>> DeleteRangeExam(IEnumerable<Guid> ids);
        Task<OperationResult<ExamDetailsDto>> AddExam(ExamDto dto);
        Task<OperationResult<ExamDetailsDto>> UpdateExam(ExamDto dto);
        Task<OperationResult<IEnumerable<ExamDetailsQuestionDto>>> GetAllExamQuestion(Guid id);

        Task<OperationResult<IEnumerable<ExamDetailsDto>>> GetAllBank();
        Task<OperationResult<bool>> DeleteBank(Guid id);
        Task<OperationResult<bool>> DeleteRangeBank(IEnumerable<Guid> ids);
        Task<OperationResult<ExamDetailsDto>> AddBank(ExamDto dto);
        Task<OperationResult<ExamDetailsDto>> UpdateBank(ExamDto dto);
        Task<OperationResult<IEnumerable<ExamDetailsQuestionDto>>> GetAllBankQuestion(Guid id);

        Task<OperationResult<IEnumerable<ExamDetailsDto>>> GetAllInterview();
        Task<OperationResult<bool>> DeleteInterview(Guid id);
        Task<OperationResult<bool>> MultiDeleteInterview(IEnumerable<Guid> ids);
        Task<OperationResult<ExamDetailsDto>> AddInterview(ExamDto dto);
        Task<OperationResult<ExamDetailsDto>> UpdateInterview(ExamDto dto);
        Task<OperationResult<IEnumerable<ExamDetailsQuestionDto>>> GetAllInterviewQuestion(Guid id);

        Task<OperationResult<IEnumerable<ExamDetailsDto>>> GetAllMicroscope();
        Task<OperationResult<MicroscopeDocumentsDto>> DetailsMicroscope(Guid id);
        Task<OperationResult<bool>> DeleteMicroscope(Guid id);
        Task<OperationResult<bool>> DeleteRangeMicroscope(IEnumerable<Guid> ids);
        Task<OperationResult<ExamDetailsDto>> AddMicroscope(ExamDto dto);
        Task<OperationResult<ExamDetailsDto>> UpdateMicroscope(ExamDto dto);
        Task<OperationResult<MicroscopeDocumentsDto>> UpdateSectionsMicroscope(SectionsMicroscopeDocumentsDto dto);
        Task<OperationResult<bool>> DeleteSectionsMicroscope(Guid id);


        Task<OperationResult<ExamDocumentDto>> AddExamDocument(ExamDocumentDto dto);

        Task<OperationResult<bool>> DeleteExamDocument(Guid documentId);

        Task<OperationResult<bool>> DeleteRangeExamDocument(IEnumerable<Guid> documentIds);
            
    }
}
