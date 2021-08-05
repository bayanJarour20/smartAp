using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.MVC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.FacultyDto;
using SmartStart.Repository.General.FacultyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FacultyController : ElControllerBase<Guid, IFacultyRepository>
    {
        public FacultyController(IFacultyRepository repository) : base(repository) { }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => await repository.GetAll().ToJsonResultAsync();

        [HttpPost]
        public async Task<IActionResult> SetFaculty([FromForm]FacultyDto facultyDto)
            => await repository.SetFaculty(facultyDto, facultyDto.File).ToJsonResultAsync();

        [HttpDelete]
        public async Task<IActionResult> RemoveFaculty(Guid facultyId)
            => await repository.RemoveFaculty(facultyId).ToJsonResultAsync();

        [HttpPut]
        public async Task<IActionResult> RemoveFaculties(List<Guid> facultyIds)
            => await repository.RemoveFaculties(facultyIds).ToJsonResultAsync();
    }
}
