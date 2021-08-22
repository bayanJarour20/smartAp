using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext.General;
using Elkood.Web.Service.DependencyInjection;
using SmartStart.DataTransferObject.CityDto;
using SmartStart.Model.General;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.BoundedContext.Store;

namespace SmartStart.Repository.General.CityService
{
    [ElRepository]
    public class CityRepository : ElRepository<SmartStartDbContext, Guid, City, _FakeStore, CityDto>, ICityRepository
    {


        public CityRepository(SmartStartDbContext context) : base(context) { }


        public async Task<OperationResult<bool>> DeleteRange(IEnumerable<Guid> ids)
           => await RepositoryHandler(_deleteRange(ids));

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _deleteRange(IEnumerable<Guid> ids)
            => async operation =>
            {

                ids.ToList().ForEach(id =>
                {
                    Context.SoftDelete<City>(id);
                });


                await Context.SaveChangesAsync(); ;

                return operation.SetSuccess(true);
            };
    }
}
