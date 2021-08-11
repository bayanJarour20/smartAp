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
    }
}
