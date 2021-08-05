using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext.General;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.UniverstiyDto;
using SmartStart.Model.General;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.General.UniversityService
{
    [ElRepository]
    public class UniversityRepository : ElRepositoryGeneral<SmartStartDbContext, Guid, University, UniversityDto>, IUniversityRepository
    {
        public UniversityRepository(SmartStartDbContext context) : base(context) { }

        public async Task<OperationResult<IEnumerable<UniversityDto>>> GetAll()
            => await RepositoryHandler(_getAll());

        public async Task<OperationResult<UniversityDto>> Add(UniversityDto universityDto)
           => await RepositoryHandler(_add(universityDto));


        public async Task<OperationResult<UniversityDto>> Update(UniversityDto universityDto)
            => await RepositoryHandler(_update(universityDto));

        public async Task<OperationResult<bool>> Delete(Guid id)
            => await RepositoryHandler(_delete(id));

        public async Task<OperationResult<bool>> DeleteRange(IEnumerable<Guid> ids)
           => await RepositoryHandler(_deleteRange(ids));



        private Func<OperationResult<IEnumerable<UniversityDto>>, Task<OperationResult<IEnumerable<UniversityDto>>>> _getAll()
           => async operation =>
           {

               var allUniversities = await Query.Select(university => new UniversityDto
               {
                   Id = university.Id,
                   CityId = university.CityId,
                   Name = university.Name
               })
               .ToListAsync();

               return operation.SetSuccess(allUniversities);
           };


        Func<string, bool> sad = x => true;
        public Func<string, bool> sadasda()
            => sad =>
            {
                return true;
            };


        private Func<OperationResult<UniversityDto>, Task<OperationResult<UniversityDto>>> _add(UniversityDto universityDto)
            => async operation =>
            {


                        var universityModel = new University()
                        {
                            Name = universityDto.Name,
                            CityId = universityDto.CityId
                        };

                        await Context.AddAsync(universityModel);
                        await Context.SaveChangesAsync();

                        universityDto.Id = universityModel.Id;

                        return operation.SetSuccess(universityDto);

            };

        private Func<OperationResult<UniversityDto>, Task<OperationResult<UniversityDto>>> _update(UniversityDto universityDto)
          => async operation =>
          {
              University universityModel = await TrackingQuery.Where(e => e.Id == universityDto.Id).FirstOrDefaultAsync();

              if (universityModel is null)
                  return OperationResultTypes.NotExist;

              universityModel.Name = universityDto.Name;
              universityModel.CityId = universityDto.CityId;


              await Context.SaveChangesAsync();

              return operation.SetSuccess(universityDto);
          };


        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _delete(Guid id)
            => async operation =>
            {
                await SoftDeleteThenSaveChangesAsync<University>(id);
                // await SoftDeleteThenSaveChangesAsync<University>(x=>x.Id==id);

                //Context.SoftDelete(await FindAsync(id));

                return operation.SetSuccess(true);
            };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _deleteRange(IEnumerable<Guid> ids)
            => async operation =>
            {

                ids.ToList().ForEach(id =>
                {
                     Context.SoftDelete<University>(id);
                });


                await Context.SaveChangesAsync(); ;

                return operation.SetSuccess(true);
            };
    }
}
