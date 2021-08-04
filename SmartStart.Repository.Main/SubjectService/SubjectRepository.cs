using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.BoundedContext.General;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.SubjectDto;
using SmartStart.Model.Main;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Main.SubjectService
{
    [ElRepository]
    public class SubjectRepository : ElRepository<SmartStartDbContext, Guid, Subject>, ISubjectRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public SubjectRepository(SmartStartDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<OperationResult<IEnumerable<SubjectDto>>> GetAll(int? year, Guid? semesterId, Guid? facultyId)
            => await RepositoryHandler(_getAll(year, semesterId, facultyId));


        private Func<OperationResult<IEnumerable<SubjectDto>>, Task<OperationResult<IEnumerable<SubjectDto>>>> _getAll(int? year, Guid? semesterId, Guid? facultyId)
         => async operation => {

             //var res = Query.Where(s => s.fa); 
             return null; 
             //return operation.SetSuccess(new SubjectDto { });
         };
    }
}
