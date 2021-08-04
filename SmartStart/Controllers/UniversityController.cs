using Elkood.Web.MVC;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.UniverstiyDto;
using SmartStart.Repository.General.UniversityService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UniversityController : ElControllerBase<Guid, IUniversityRepository>
    {
        public UniversityController(IUniversityRepository repository) : base(repository) { }
    }
}
