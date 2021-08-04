using Elkood.Web.Service.BoundedContext.General;
using SmartStart.DataTransferObject.UniverstiyDto;
using SmartStart.Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.General.UniversityService
{
    public interface IUniversityRepository : IElRepositoryGeneral<Guid, University, UniversityDto>
    {
    }
}
