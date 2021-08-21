using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.GeneralDto;
using SmartStart.Model.Main;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Main.GeneralServices
{
    [ElRepository]
    public class GeneralRepository : ElRepository<SmartStartDbContext, Guid>, IGeneralRepository
    {
        public GeneralRepository(SmartStartDbContext context) : base(context) { }

        public async Task<OperationResult<object>> GetRemaining(Guid UserId)
            => await RepositoryHandler(_getRemaining(UserId));
        public async Task<OperationResult<bool>> SetSelected(SelectedDto selectedDto, Guid UserId)
            => await RepositoryHandler(_setSelected(selectedDto, UserId));

        private Func<OperationResult<object>, Task<OperationResult<object>>> _getRemaining(Guid UserId)
            => async operation =>
            {
                object res = _query<SubjectFaculty>().Include(s => s.Section)
                                                     .Include(s => s.Semester)
                                                     .Include(s => s.Faculty)
                                                     .Where(s => !s.SubjectFacultyAppUsers.Where(ss => ss.AppUserId == UserId
                                                                                                    && ss.DefaultSelected).Any())
                                                     .ToList()
                                                     .GroupBy(s => new { s.FacultyId, s.Faculty.Name })
                                                     .Select(s => new
                                                     {
                                                         FacultyId = s.Key.FacultyId,
                                                         FacultyName = s.Key.Name,
                                                         Sections = s.GroupBy(s2 => new { s2.SectionId, s2.Section.Name })
                                                                          .Select(s2 => new
                                                                          {
                                                                              SectionId = s2.Key.SectionId,
                                                                              SectionName = s2.Key.Name,
                                                                              Years = s2.GroupBy(s3 => s3.Year)
                                                                                              .Select(s3 => new
                                                                                              {
                                                                                                  Year = s3.Key,
                                                                                                  Semesters = s3.Select(s4 => new
                                                                                                  {
                                                                                                      SemesterId = s4.SemesterId,
                                                                                                      SemesterName = s4.Semester.Name
                                                                                                  }).ToList()
                                                                                              }).ToList()
                                                                          }).ToList()
                                                     }).ToList();
                return operation.SetSuccess(res);
            };
        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _setSelected(SelectedDto selectedDto, Guid UserId)
          => async operation =>
          {
              var SubjectFacultyId = (await _query<SubjectFaculty>().SingleOrDefaultAsync(s => s.FacultyId == selectedDto.FacultyId
                                                                                                 && s.SectionId == selectedDto.SectionId
                                                                                                 && s.Year == selectedDto.Year
                                                                                                 && s.SemesterId == selectedDto.SemesterId)).Id;
              await Context.SubjectFacultyAppUsers.AddAsync(new SubjectFacultyAppUser
              {
                  AppUserId = UserId,
                  SubjectFacultyId = SubjectFacultyId,
                  DefaultSelected = true
              });
              await Context.SaveChangesAsync(); 
              return operation.SetSuccess(true);
          };
    }
}
