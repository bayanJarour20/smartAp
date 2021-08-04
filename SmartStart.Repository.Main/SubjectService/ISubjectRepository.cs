using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using SmartStart.DataTransferObject.SubjectDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Main.SubjectService
{
    public interface ISubjectRepository : IElRepository
    {
        Task<OperationResult<IEnumerable<SubjectDto>>> GetAll(int? year, Guid? semesterId, Guid? facultyId);
        Task<OperationResult<SubjectDto>> SetSubject(SubjectDetailsDto subjectDto);
        Task<OperationResult<SubjectAllDto>> SubjectDetails(Guid subjectId);
        Task<OperationResult<bool>> RemoveSubject(Guid subjectId);
    }
}
