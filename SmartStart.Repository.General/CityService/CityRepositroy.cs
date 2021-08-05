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

namespace SmartStart.Repository.General.CityService
{
    [ElRepository]
    public class CityRepositroy : ElRepositoryGeneral<SmartStartDbContext, Guid, City, CityDto>, ICityRepositroy
    {
        public CityRepositroy(SmartStartDbContext context) : base(context)
        {
        }

        public async Task<OperationResult<IEnumerable<CityDto>>> GetCities()
            => await RepositoryHandler(_getCiteies);


        private Func<OperationResult<IEnumerable<CityDto>>, Task<OperationResult<IEnumerable<CityDto>>>> _getCiteies
            => async operation => {
                var list = await Query
                                    .Select(city => new CityDto()
                                    {
                                        Id = city.Id,
                                        Name = city.Name
                                    })
                                    .ToListAsync();
                return operation.SetSuccess(list);
            };
    }
}
