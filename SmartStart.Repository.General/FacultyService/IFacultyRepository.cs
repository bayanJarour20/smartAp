using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using Microsoft.AspNetCore.Http;
using SmartStart.DataTransferObject.FacultyDto;
using SmartStart.DataTransferObject.SharedDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.General.FacultyService
{
    public interface IFacultyRepository : IElRepository
    {
        Task<OperationResult<IEnumerable<FacultyBaseDto>>> GetAll();
        Task<OperationResult<FacultyDto>> SetFaculty(FacultyDto facultyDto, IFormFile formFile);
        Task<OperationResult<bool>> RemoveFaculty(Guid facultyId);
        Task<OperationResult<bool>> RemoveFaculties(List<Guid> facultyIds);
        Task<OperationResult<IEnumerable<SelectDto>>> FacultySelect();
    }
}
