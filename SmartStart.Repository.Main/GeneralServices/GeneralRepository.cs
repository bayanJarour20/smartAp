using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.GeneralDto;
using SmartStart.Model.Business;
using SmartStart.Model.Main;
using SmartStart.Model.Shared;
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
        public async Task<OperationResult<bool>> RemoveSelected(SelectedDto selectedDto, Guid UserId)
            => await RepositoryHandler(_removeSelected(selectedDto, UserId));
        public async Task<OperationResult<object>> GetSelected(Guid UserId)
            => await RepositoryHandler(_getSelected(UserId));
        public async Task<OperationResult<object>> ActivateCode(string Code, Guid UserId)
            => await RepositoryHandler(_activateCode(Code, UserId));
        private Func<OperationResult<object>, Task<OperationResult<object>>> _getRemaining(Guid UserId)
            => async operation =>
            {
                object res = _query<SubjectFaculty>().Include(s => s.Section)
                                                     .Include(s => s.Semester)
                                                     .Include("Faculty.University")
                                                     .Include("Subject.Exams.ExamQuestions.Question.QuestionTags")
                                                     .Include("Subject.Exams.ExamQuestions.Question.QuestionDocuments")
                                                     .Include("Subject.Exams.ExamQuestions.Question.Answers")
                                                     .Where(s => !s.SubjectFacultyAppUsers.Where(ss => ss.AppUserId == UserId
                                                                                                    && ss.DefaultSelected).Any())
                                                     .ToList()
                                                     .GroupBy(s => new { s.FacultyId, s.Faculty.Name, UniversityName = s.Faculty.University.Name })
                                                     .Select(s => new
                                                     {
                                                         FacultyId = s.Key.FacultyId,
                                                         FacultyName = s.Key.Name + " - " + s.Key.UniversityName,
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
                                                                                                      SelectedId = s4.Id
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
                                                                    .Include("Subject.SubjectTags.Tag")
                                                                    .Include("Subject.Exams.ExamDocuments.Document")
                                                                    .Include("Subject.Exams.ExamTags.Tag")
                                                                    .Include("Subject.Exams.ExamQuestions.Question.QuestionTags")
                                                                    .Include("Subject.Exams.ExamQuestions.Question.QuestionDocuments")
                                                                    .Include("Subject.Exams.ExamQuestions.Question.Answers")
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
                  res.Add(fillDto(subjectFaculty.Subject, subjectFaculty.PackageSubjectFaculties.ToList(), UserId));
              }
              await Context.SaveChangesAsync();
              return operation.SetSuccess(new
              {
                  Subjects = res,
                  SubjectFaculty = new
                  {
                      FacultyId = SubjectFaculties.First().FacultyId,
                      FacultyName = SubjectFaculties.First().Faculty.Name,
                      SectionId = SubjectFaculties.First().SectionId,
                      SectionName = SubjectFaculties.First().Section.Name,
                      SemesterId = SubjectFaculties.First().SemesterId,
                      SemesterName = SubjectFaculties.First().Semester.Name,
                      Year = SubjectFaculties.First().Year,
                      SelectedId = SubjectFaculties.First().Id
                  },
                  Tags = _query<Tag>().Select(t => new
                  {
                      Id = t.Id,
                      Name = t.Name, 
                      Type = t.Type
                  }).ToList()
              });
          };
        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _removeSelected(SelectedDto selectedDto, Guid UserId)
          => async operation =>
          {
              var SubjectFacultyAppUsers = (await _query<SubjectFacultyAppUser>().Where(s => s.SubjectFaculty.FacultyId == selectedDto.FacultyId
                                                                                    && s.SubjectFaculty.SectionId == selectedDto.SectionId
                                                                                    && s.SubjectFaculty.Year == selectedDto.Year
                                                                                    && s.SubjectFaculty.SemesterId == selectedDto.SemesterId
                                                                                    && s.AppUserId == UserId).ToListAsync());
              if (SubjectFacultyAppUsers == null || SubjectFacultyAppUsers.Count() == 0)
              {
                  return operation.SetSuccess(false);
              }

              Context.SubjectFacultyAppUsers.RemoveRange(SubjectFacultyAppUsers);
              
              await Context.SaveChangesAsync();
              
              return operation.SetSuccess(true);
          };
        private Func<OperationResult<object>, Task<OperationResult<object>>> _getSelected(Guid UserId)
            => async operation =>
            {
                var res = _query<SubjectFaculty>().Include(s => s.Section)
                                                  .Include(s => s.Semester)
                                                  .Include("Faculty.University")
                                                  .Include("Subject.SubjectTags.Tag")
                                                  .Include("Subject.Exams.ExamDocuments.Document")
                                                  .Include("Subject.Exams.ExamTags.Tag")
                                                  .Include("Subject.Exams.ExamQuestions.Question.QuestionTags")
                                                  .Include("Subject.Exams.ExamQuestions.Question.QuestionDocuments")
                                                  .Include("Subject.Exams.ExamQuestions.Question.Answers")
                                                  .Where(s => s.SubjectFacultyAppUsers.Where(a => a.AppUserId == UserId).Any())
                                                  .ToList()
                                                  .GroupBy(s => new { s.FacultyId, s.Faculty.Name, UniversityName = s.Faculty.University.Name })
                                                  .Select(s => new
                                                  {
                                                      FacultyId = s.Key.FacultyId,
                                                      FacultyName = s.Key.Name + " - " + s.Key.UniversityName,
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
                                                                                                SelectedId = s4.Id,
                                                                                                Subjects = s.Select(p => fillDto(p.Subject, p.PackageSubjectFaculties.ToList(), UserId)).ToList()
                                                                                            }).ToList()
                                                                                        }).ToList()
                                                                    }).ToList()
                                                  }).ToList();
                return operation.SetSuccess(new
                {
                    Data = res,
                    Tags = _query<Tag>().Select(t => new
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Type = t.Type
                    }).ToList()
                });
            };
        private Func<OperationResult<object>, Task<OperationResult<object>>> _activateCode(string Hash, Guid UserId)
          => async operation =>
          {
              var code = await _query<Code>().Include("CodePackages.Package.PackageSubjectFaculties.SubjectFaculty")
                                             .Where(code => code.Hash == Hash
                                                         && !code.DateActivated.HasValue
                                                         && !code.UserId.HasValue)
                                             .SingleOrDefaultAsync();

              if (code is null || code.MaxEndDate <= DateTime.Now.ToLocalTime())
                  return operation.SetFailed("Wrong code");
              
              code.UserId = UserId;
              code.DateActivated = DateTime.Now.ToLocalTime();
              
              var subjectIds = code.CodePackages.SelectMany(x => x.Package.PackageSubjectFaculties.Select(x => x.SubjectFacultyId));

              var temp = subjectIds.Except(await _query<SubjectFacultyAppUser>().Include(s => s.SubjectFaculty)
                                                                                .ThenInclude(s => s.PackageSubjectFaculties)
                                                                                .Where(s => s.AppUserId == Context.CurrentUserId)
                                                                                        .Select(s => s.SubjectFacultyId)
                                                                                        .ToListAsync())
                                                                                        .Select(s => new SubjectFacultyAppUser
                                                                                        {
                                                                                            SubjectFacultyId = s,
                                                                                            AppUserId = UserId,
                                                                                        }).ToList();
              Context.AddRange(temp);
              var res = _query<SubjectFaculty>().Where(s => temp.Where(t => t.SubjectFacultyId == s.Id).Any())
                                                .Include(s => s.Section)
                                                .Include(s => s.Semester)
                                                .Include("Faculty.University")
                                                .Include("Subject.SubjectTags.Tag")
                                                .Include("Subject.Exams.ExamDocuments.Document")
                                                .Include("Subject.Exams.ExamTags.Tag")
                                                .Include("Subject.Exams.ExamQuestions.Question.QuestionTags")
                                                .Include("Subject.Exams.ExamQuestions.Question.QuestionDocuments")
                                                .Include("Subject.Exams.ExamQuestions.Question.Answers")
                                                .ToList()
                                                .GroupBy(s => new { s.FacultyId, s.Faculty.Name, UniversityName = s.Faculty.University.Name })
                                                .Select(s => new
                                                {
                                                    FacultyId = s.Key.FacultyId,
                                                    FacultyName = s.Key.Name + " - " + s.Key.UniversityName,
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
                                                                                            SelectedId = s4.Id,
                                                                                            Subjects = s.Select(p => fillDto(p.Subject, p.PackageSubjectFaculties.ToList(), UserId)).ToList()
                                                                                        }).ToList()
                                                                                    }).ToList()
                                                                }).ToList()
                                                }).ToList();
              return operation.SetSuccess(res);
          };

        #region Helper Function Section
        private object fillDto(Subject p, List<PackageSubjectFaculty> ps, Guid UserId)
        {
            return new
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                ImagePath = p.ImagePath,
                SubjectTags = p.SubjectTags.Select(t => t.TagId).ToList(),
                Type = p.Type,
                IsActive = ps.Where(q => q.Package.CodePackages.Where(c => c.Code.UserId == UserId).Any()).Any(),
                Exams = p.Exams.Where(e => e.Type == TabTypes.Exam)
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
                                       ExamTags = ee.ExamTags.Select(t => t.TagId).ToList(),
                                       ExamDocuments = ee.ExamDocuments.Select(t => new
                                       {
                                           Name = t.Document.Name,
                                           Path = t.Document.Path,
                                           Type = t.Document.Type
                                       }),
                                       ExamQuestions = ee.ExamQuestions.Select(q => new
                                       {
                                           QuestionId = q.QuestionId,
                                           Order = q.Order,
                                           Title = q.Question.Title,
                                           Hint = q.Question.Hint,
                                           IsCorrected = q.Question.IsCorrected,
                                           QuestionType = q.Question.QuestionType,
                                           AnswerType = q.Question.AnswerType,
                                           QuestionTags = q.Question.QuestionTags.Select(t => t.TagId).ToList(),
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
                                               IsCorrect = a.IsCorrect,
                                               CorrectionDate = a.CorrectionDate
                                           })
                                       })
                                   })
                               }).ToList()

            };
        }
        #endregion
    }
}
