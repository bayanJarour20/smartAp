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

        public async Task<OperationResult<CityDto>> Add(CityDto cityDto)
           => await RepositoryHandler(_add(cityDto));

        public async Task<OperationResult<CityDto>> Update(CityDto cityDto)
           => await RepositoryHandler(_update(cityDto));

        public async Task<OperationResult<bool>> Delete(Guid id)
            => await RepositoryHandler(_delete(id));

        public async Task<OperationResult<bool>> DeleteRange(IEnumerable<Guid> ids)
           => await RepositoryHandler(_deleteRange(ids));

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



        private Func<OperationResult<CityDto>, Task<OperationResult<CityDto>>> _add(CityDto cityDto)
            => async operation =>
            {


                var cityModel = new City()
                {
                    Name = cityDto.Name,
                };

                await Context.AddAsync(cityModel);
                await Context.SaveChangesAsync();

                cityDto.Id = cityModel.Id;

                return operation.SetSuccess(cityDto);

            };


        private Func<OperationResult<CityDto>, Task<OperationResult<CityDto>>> _update(CityDto cityDto)
         => async operation =>
         {
             City cityModel = await TrackingQuery.Where(e => e.Id == cityDto.Id).FirstOrDefaultAsync();

             if (cityModel is null)
                 return OperationResultTypes.NotExist;

             cityModel.Name = cityDto.Name;


             await Context.SaveChangesAsync();

             return operation.SetSuccess(cityDto);
         };


        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _delete(Guid id)
            => async operation =>
            {
                await SoftDeleteThenSaveChangesAsync<City>(id);


                return operation.SetSuccess(true);
            };

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
