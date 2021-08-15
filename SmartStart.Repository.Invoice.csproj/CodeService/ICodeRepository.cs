using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using SmartStart.DataTransferObject.CodeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Invoice.CodeService
{
    public interface ICodeRepository : IElRepository
    {
        Task<OperationResult<IEnumerable<CodeSubjectsPriceDto>>> GetCodes(Guid userId);
        Task<OperationResult<bool>> ActivateCode(Guid userId, string hash);
        Task<OperationResult<bool>> ActivateCodeV2(string hash, IEnumerable<Guid> subjectFacultyIds);
        Task<OperationResult<object>> CheckCode(string hash);


        Task<OperationResult<IEnumerable<CodeDetailsDto>>> GetAll();
        Task<OperationResult<CodeDetailsDto>> Generate(CodeGenerateDto dto);
        Task<OperationResult<CodeDetailsDto>> Update(CodeGenerateDto dto);
        Task<OperationResult<bool>> Delete(Guid id);
        Task<OperationResult<bool>> RemoveCodes(List<Guid> codeIds);
    }
}
