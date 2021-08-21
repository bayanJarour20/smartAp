using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.GeneralDto;
using SmartStart.Model.Main;
using SmartStart.SharedKernel.Enums;
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
        public async Task<OperationResult<object>> SetSelected(SelectedDto selectedDto, Guid UserId)
            => await RepositoryHandler(_setSelected(selectedDto, UserId));
        public async Task<OperationResult<object>> GetSelected(Guid UserId)
            => await RepositoryHandler(_getSelected(UserId));

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
        private Func<OperationResult<object>, Task<OperationResult<object>>> _setSelected(SelectedDto selectedDto, Guid UserId)
          => async operation =>
          {
              var SubjectFaculties = (await _query<SubjectFaculty>().Include(s => s.Section)
                                                                    .Include(s => s.Semester)
                                                                    .Include(s => s.Faculty)
                                                                    .Include(s => s.Subject)
                                                                    .ThenInclude(t => t.SubjectTags)
                                                                    .ThenInclude(t => t.Tag)
                                                                    .Include(s => s.Subject)
                                                                    .ThenInclude(t => t.Exams)
                                                                    .ThenInclude(t => t.ExamDocuments)
                                                                    .ThenInclude(t => t.Document)
                                                                    .Include(s => s.Subject)
                                                                    .ThenInclude(t => t.Exams)
                                                                    .ThenInclude(t => t.ExamTags)
                                                                    .ThenInclude(t => t.Tag)
                                                                    .Where(s => s.FacultyId == selectedDto.FacultyId
                                                                                            && s.SectionId == selectedDto.SectionId
                                                                                            && s.Year == selectedDto.Year
                                                                                            && s.SemesterId == selectedDto.SemesterId).ToListAsync());
              var res = new List<object>(); 
              foreach (var subjectFaculty in SubjectFaculties)
              {
                  await Context.SubjectFacultyAppUsers.AddAsync(new SubjectFacultyAppUser
                  {
                      AppUserId = UserId,
                      SubjectFacultyId = subjectFaculty.Id,
                      DefaultSelected = true
                  });
                  res.Add(fillDto(subjectFaculty, UserId));
              }
              await Context.SaveChangesAsync();
              return operation.SetSuccess(res);
          };
        private object fillDto(SubjectFaculty p, Guid UserId)
        {
            return new 
            {
                Name = p.Subject.Name,
                Description = p.Subject.Description,
                ImagePath = p.Subject.ImagePath,
                SubjectTags = p.Subject.SubjectTags
                                                .Select(t => new
                                                {
                                                    TagId = t.TagId,
                                                    TagName = t.Tag.Name,
                                                    Type = t.Tag.Type
                                                }).ToList(),
                Type = p.Subject.Type,
                IsActive = p.PackageSubjectFaculties.Where(q => q.Package.CodePackages.Where(c => c.Code.UserId == UserId).Any()).Any(),
                Exams = p.Subject.Exams.Where(e => e.Type == TabTypes.Exam)
                                       .GroupBy(e => e.Type)
                                       .Select(e => new
                                       {
                                           Type = e.Key,
                                           Exams = e.Select(ee => new
                                           {
                                               Id = ee.Id,
                                               Name = ee.Name,
                                               Price = ee.Price,
                                               Year = ee.Year,
                                               ExamTags = ee.ExamTags.Select(t => new
                                               {
                                                   TagId = t.TagId,
                                                   TagName = t.Tag.Name,
                                                   Type = t.Tag.Type
                                               }).ToList(),
                                               ExamDocuments = ee.ExamDocuments.Select(t => new
                                               {
                                                   Name = t.Document.Name,
                                                   Path = t.Document.Path,
                                                   Type = t.Document.Type
                                               }),
                                               ExamQuestions = ee.ExamQuestions.Select(q => new
                                               {
                                                   Order = q.Order,
                                                   Title = q.Question.Title,
                                                   Hint = q.Question.Hint,
                                                   IsCorrected = q.Question.IsCorrected,
                                                   QuestionType = q.Question.QuestionType,
                                                   AnswerType = q.Question.AnswerType,
                                                   QuestionTags = q.Question.QuestionTags.Select(t => new
                                                   {
                                                       TagId = t.TagId,
                                                       TagName = t.Tag.Name,
                                                       Type = t.Tag.Type
                                                   }).ToList(),
                                                   QuestionDocuments = q.Question.QuestionDocuments.Select(d => new
                                                   {
                                                       DocumentId = d.DocumentId,
                                                       Note = d.Note,
                                                       Name = d.Document.Name,
                                                       Path = d.Document.Path,
                                                       Type = d.Document.Type
                                                   }),
                                                   Answers = q.Question.Answers.Select(a => new
                                                   {
                                                       Id = a.Id,
                                                       Title = a.Title,
                                                       Option = a.Option,
                                                       IsCorrect = a.IsCorrect,
                                                       CorrectionDate = a.CorrectionDate
                                                   })
                                               })
                                           })
                                       }).ToList()

            };
        }
        private Func<OperationResult<object>, Task<OperationResult<object>>> _getSelected(Guid UserId)
            => async operation =>
            {
                var res = _query<SubjectFaculty>().Include(s => s.Section)
                                                  .Include(s => s.Semester)
                                                  .Include(s => s.Faculty)
                                                  .Include(s => s.Subject)
                                                  .ThenInclude(t => t.SubjectTags)
                                                  .ThenInclude(t => t.Tag)
                                                  .Include(s => s.Subject)
                                                  .ThenInclude(t => t.Exams)
                                                  .ThenInclude(t => t.ExamDocuments)
                                                  .ThenInclude(t => t.Document)
                                                  .Include(s => s.Subject)
                                                  .ThenInclude(t => t.Exams)
                                                  .ThenInclude(t => t.ExamTags)
                                                  .ThenInclude(t => t.Tag)
                                                  .Where(s => s.SubjectFacultyAppUsers.Where(a => a.AppUserId == UserId).Any())
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
                                                                                                SemesterName = s4.Semester.Name,
                                                                                                Subjects = s.Select(p => fillDto(p, UserId)).ToList()
                                                                                            }).ToList()
                                                                                        }).ToList()
                                                                    }).ToList()
                                                  }).ToList();
                return operation.SetSuccess(res);
            };
    }
}
