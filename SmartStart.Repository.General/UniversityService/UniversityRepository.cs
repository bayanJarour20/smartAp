using Elkood.Web.Service.BoundedContext.General;
using Elkood.Web.Service.DependencyInjection;
using SmartStart.DataTransferObject.UniverstiyDto;
using SmartStart.Model.General;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.General.UniversityService
{
    [ElRepository]
    public class UniversityRepository : ElRepositoryGeneral<SmartStartDbContext, Guid, University, UniversityDto>, IUniversityRepository
    {
        public UniversityRepository(SmartStartDbContext context) : base(context) { }
    }
}
