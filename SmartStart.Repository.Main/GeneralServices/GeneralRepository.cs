﻿using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.GeneralDto;
using SmartStart.Model.Business;
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
                                                     .Include(s => s.Faculty)
                                                     .ThenInclude(s => s.University)
                                                     .Include(s => s.Subject)
                                                     .ThenInclude(t => t.Exams)
                                                     .ThenInclude(t => t.ExamQuestions)
                                                     .ThenInclude(t => t.Question)
                                                     .ThenInclude(t => t.QuestionTags)
                                                     .Include(s => s.Subject)
                                                     .ThenInclude(t => t.Exams)
                                                     .ThenInclude(t => t.ExamQuestions)
                                                     .ThenInclude(t => t.Question)
                                                     .ThenInclude(t => t.QuestionDocuments)
                                                     .Include(s => s.Subject)
                                                     .ThenInclude(t => t.Exams)
                                                     .ThenInclude(t => t.ExamQuestions)
                                                     .ThenInclude(t => t.Question)
                                                     .ThenInclude(t => t.Answers)
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
                  res.Add(fillDto(subjectFaculty.Subject, subjectFaculty.PackageSubjectFaculties.ToList(), UserId));
              }
              await Context.SaveChangesAsync();
              return operation.SetSuccess(res);
          };
        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _removeSelected(SelectedDto selectedDto, Guid UserId)
          => async operation =>
          {

              var SubjectFacultyAppUsers = (await _query<SubjectFacultyAppUser>().Where(s => s.SubjectFaculty.FacultyId == selectedDto.FacultyId
                                                                                    && s.SubjectFaculty.SectionId == selectedDto.SectionId
                                                                                    && s.SubjectFaculty.Year == selectedDto.Year
                                                                                    && s.SubjectFaculty.SemesterId == selectedDto.SemesterId
                                                                                    && s.AppUserId == UserId).ToListAsync());
              Context.SubjectFacultyAppUsers.RemoveRange(SubjectFacultyAppUsers);
              
              await Context.SaveChangesAsync();
              
              return operation.SetSuccess(true);
          };
        private Func<OperationResult<object>, Task<OperationResult<object>>> _getSelected(Guid UserId)
            => async operation =>
            {
                var res = _query<SubjectFaculty>().Include(s => s.Section)
                                                  .Include(s => s.Semester)
                                                  .Include(s => s.Faculty)
                                                  .ThenInclude(s => s.University)
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
                                                  .Include(s => s.Subject)
                                                  .ThenInclude(t => t.Exams)
                                                  .ThenInclude(t => t.ExamQuestions)
                                                  .ThenInclude(t => t.Question)
                                                  .ThenInclude(t => t.QuestionTags)
                                                  .Include(s => s.Subject)
                                                  .ThenInclude(t => t.Exams)
                                                  .ThenInclude(t => t.ExamQuestions)
                                                  .ThenInclude(t => t.Question)
                                                  .ThenInclude(t => t.QuestionDocuments)
                                                  .Include(s => s.Subject)
                                                  .ThenInclude(t => t.Exams)
                                                  .ThenInclude(t => t.ExamQuestions)
                                                  .ThenInclude(t => t.Question)
                                                  .ThenInclude(t => t.Answers)
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
                                                                                                Subjects = s.Select(p => fillDto(p.Subject, p.PackageSubjectFaculties.ToList(), UserId)).ToList()
                                                                                            }).ToList()
                                                                                        }).ToList()
                                                                    }).ToList()
                                                  }).ToList();
                return operation.SetSuccess(res);
            };
        private Func<OperationResult<object>, Task<OperationResult<object>>> _activateCode(string Hash, Guid UserId)
          => async operation =>
          {
              var code = await _query<Code>().Include(x => x.CodePackages)
                                             .ThenInclude(x => x.Package)
                                             .ThenInclude(x => x.PackageSubjectFaculties)
                                             .ThenInclude(x => x.SubjectFaculty)
                                             .Where(code => code.Hash == Hash
                                                         && !code.DateActivated.HasValue
                                                         && !code.UserId.HasValue)
                                             .SingleOrDefaultAsync();

              if (code is null || code.MaxEndDate <= DateTime.Now.ToLocalTime())
                  return operation.SetFailed("Wrong code");
              
              code.UserId = UserId;
              code.DateActivated = DateTime.Now.ToLocalTime();
              
              var subjectIds = code.CodePackages.SelectMany(x => x.Package.PackageSubjectFaculties.Select(x => x.SubjectFaculty.SubjectId));

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
              var res = new List<object>(); 
              foreach (var item in temp)
              {
                  res.Add(fillDto(item.SubjectFaculty.Subject, item.SubjectFaculty.PackageSubjectFaculties.ToList(), UserId));
              }
              return operation.SetSuccess(res);
          };

        #region Helper Function Section
        private object fillDto(Subject p, List<PackageSubjectFaculty> ps, Guid UserId)
        {
            return new
            {
                Name = p.Name,
                Description = p.Description,
                ImagePath = p.ImagePath,
                SubjectTags = p.SubjectTags.Select(t => new
                {
                    TagId = t.TagId,
                    TagName = t.Tag.Name,
                    Type = t.Tag.Type
                }).ToList(),
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
        #endregion
    }
}