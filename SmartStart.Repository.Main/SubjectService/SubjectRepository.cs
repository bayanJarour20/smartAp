using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.BoundedContext.General;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
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
    public class SubjectRepository : ElRepository<SmartStartDbContext, Guid, Subject>
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public SubjectRepository(SmartStartDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            this.webHostEnvironment = webHostEnvironment;
        }


    }
}
