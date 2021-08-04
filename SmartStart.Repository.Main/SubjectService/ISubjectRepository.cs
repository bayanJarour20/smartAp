using Elkood.Web.Common.ContextResult.OperationContext;
using SmartStart.DataTransferObject.SubjectDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Main.SubjectService
{
    public interface ISubjectRepository
    {
        Task<OperationResult<IEnumerable<SubjectDto>>> GetAll(int? year, Guid? semesterId, Guid? facultyId);
    }
}
