﻿using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Helper.ExtensionMethods.String;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.ExamDto;
using SmartStart.DataTransferObject.QuestionDto;
using SmartStart.Model.Main;
using SmartStart.Model.Shared;
using SmartStart.SharedKernel.Enums;
using SmartStart.SharedKernel.ExtensionMethods;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Main.ExamServices
{
    [ElRepository]
    public class ExamRepository : ElRepository<SmartStartDbContext, Guid, Exam>, IExamRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public ExamRepository(SmartStartDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            this.webHostEnvironment = webHostEnvironment;
        }


        public async Task<OperationResult<IEnumerable<ExamDetailsDto>>> GetAllExam()
            => await RepositoryHandler(_getAllExam());
        public async Task<OperationResult<bool>> DeleteExam(Guid id)
            => await RepositoryHandler(_deleteExam(id));
        public async Task<OperationResult<bool>> MultiDeleteExam(IEnumerable<Guid> ids)
            => await RepositoryHandler(_multiDeleteExam(ids));
        public async Task<OperationResult<ExamDetailsDto>> AddExam(ExamDto exam)
            => await RepositoryHandler(_addExam(exam));
        public async Task<OperationResult<ExamDetailsDto>> UpdateExam(ExamDto dto)
            => await RepositoryHandler(_updateExam(dto));
        public async Task<OperationResult<IEnumerable<ExamDetailsQuestionDto>>> GetAllExamQuestion(Guid id)
            => await RepositoryHandler(_getAllExamQuestion(id));


        public async Task<OperationResult<IEnumerable<ExamDetailsDto>>> GetAllBank()
            => await RepositoryHandler(_getAllBank());
        public async Task<OperationResult<bool>> DeleteBank(Guid id)
            => await RepositoryHandler(_deleteBank(id));
        public async Task<OperationResult<bool>> MultiDeleteBank(IEnumerable<Guid> ids)
            => await RepositoryHandler(_multiDeleteBank(ids));
        public async Task<OperationResult<ExamDetailsDto>> AddBank(ExamDto dto)
            => await RepositoryHandler(_addBank(dto));
        public async Task<OperationResult<ExamDetailsDto>> UpdateBank(ExamDto dto)
            => await RepositoryHandler(_updateBank(dto));
        public async Task<OperationResult<IEnumerable<ExamDetailsQuestionDto>>> GetAllBankQuestion(Guid id)
            => await RepositoryHandler(_getAllBankQuestion(id));


        public async Task<OperationResult<IEnumerable<ExamDetailsDto>>> GetAllInterview()
            => await RepositoryHandler(_getAllInterview());
        public async Task<OperationResult<bool>> DeleteInterview(Guid id)
            => await RepositoryHandler(_deleteInterview(id));
        public async Task<OperationResult<bool>> MultiDeleteInterview(IEnumerable<Guid> ids)
            => await RepositoryHandler(_multiDeleteInterview(ids));
        public async Task<OperationResult<ExamDetailsDto>> AddInterview(ExamDto dto)
            => await RepositoryHandler(_addInterview(dto));
        public async Task<OperationResult<ExamDetailsDto>> UpdateInterview(ExamDto dto)
            => await RepositoryHandler(_updateInterview(dto));
        public async Task<OperationResult<IEnumerable<ExamDetailsQuestionDto>>> GetAllInterviewQuestion(Guid id)
            => await RepositoryHandler(_getAllInterviewQuestion(id));


        public async Task<OperationResult<IEnumerable<ExamDetailsDto>>> GetAllMicroscope()
            => await RepositoryHandler(_getAllMicroscope());
        public async Task<OperationResult<MicroscopeDocumentsDto>> DetailsMicroscope(Guid id)
            => await RepositoryHandler(_detailsMicroscope(id));
        public async Task<OperationResult<bool>> DeleteMicroscope(Guid id)
            => await RepositoryHandler(_deleteMicroscope(id));
        public async Task<OperationResult<bool>> MultiDeleteMicroscope(IEnumerable<Guid> ids)
            => await RepositoryHandler(_multiDeleteMicroscope(ids));
        public async Task<OperationResult<ExamDetailsDto>> AddMicroscope(ExamDto dto)
            => await RepositoryHandler(_addMicroscope(dto));
        public async Task<OperationResult<ExamDetailsDto>> UpdateMicroscope(ExamDto dto)
            => await RepositoryHandler(_updateMicroscope(dto));
        public async Task<OperationResult<MicroscopeDocumentsDto>> UpdateSectionsMicroscope(SectionsMicroscopeDocumentsDto dto)
            => await RepositoryHandler(_updateSectionsMicroscope(dto));
        public async Task<OperationResult<bool>> DeleteSectionsMicroscope(Guid id)
            => await RepositoryHandler(_deleteSectionsMicroscope(id));

        #region - Exam -

        private Func<OperationResult<IEnumerable<ExamDetailsDto>>, Task<OperationResult<IEnumerable<ExamDetailsDto>>>> _getAllExam()
            => async operation =>
            {
                var Exams = await GetAllAsync(exam => exam.Type == TabTypes.Exam);
                return operation.SetSuccess(Exams);
            };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _deleteExam(Guid id)
            => async operation =>
            {
                if (!(await TryDelete(id, TabTypes.Exam)))
                    return (OperationResultTypes.NotExist, $"{id} : not exist.");
                return operation.SetSuccess(true, " Success Delete. ");
            };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _multiDeleteExam(IEnumerable<Guid> ids)
            => async operation =>
            {
                if (!(await TryMultiDelete(ids, TabTypes.Exam)))
                    return (OperationResultTypes.NotExist, $"{ids} : not exist.");
                return operation.SetSuccess(true, "Success Delete All.");
            };

        private Func<OperationResult<ExamDetailsDto>, Task<OperationResult<ExamDetailsDto>>> _addExam(ExamDto dto)
            => async operation =>
            {
                var exam = await AddAsync(dto, TabTypes.Exam);
                return operation.SetSuccess(exam);
            };

        private Func<OperationResult<ExamDetailsDto>, Task<OperationResult<ExamDetailsDto>>> _updateExam(ExamDto dto)
            => async operation => await UpdateAsync(operation, dto, TabTypes.Exam);

        private Func<OperationResult<IEnumerable<ExamDetailsQuestionDto>>, Task<OperationResult<IEnumerable<ExamDetailsQuestionDto>>>> _getAllExamQuestion(Guid id)
            => async operation =>
            {
                var examQuestions = await GetAllQuestionAsync(exam => exam.Id == id && exam.Type == TabTypes.Exam);
                return operation.SetSuccess(examQuestions);
            };

        #endregion

        #region - Bank -

        private Func<OperationResult<IEnumerable<ExamDetailsDto>>, Task<OperationResult<IEnumerable<ExamDetailsDto>>>> _getAllBank()
            => async operatoin =>
            {
                var Banks = await GetAllAsync(exam => exam.Type == TabTypes.Bank);
                return operatoin.SetSuccess(Banks);
            };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _deleteBank(Guid id)
            => async operation =>
            {
                if (!(await TryDelete(id, TabTypes.Bank)))
                {
                    return (OperationResultTypes.NotExist, $"{id}: not exist. ");
                }
                return operation.SetSuccess(true, "Sussecc Delete.");
            };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _multiDeleteBank(IEnumerable<Guid> ids)
            => async operation =>
            {
                if (!(await TryMultiDelete(ids, TabTypes.Bank)))
                {
                    return (OperationResultTypes.NotExist, $"{ids}: not exist. ");
                }
                return operation.SetSuccess(true, "Sussecc Delete.");
            };

        private Func<OperationResult<ExamDetailsDto>, Task<OperationResult<ExamDetailsDto>>> _addBank(ExamDto dto)
            => async operation =>
            {
                var bank = await AddAsync(dto, TabTypes.Bank);
                return operation.SetSuccess(bank);
            };


        private Func<OperationResult<ExamDetailsDto>, Task<OperationResult<ExamDetailsDto>>> _updateBank(ExamDto dto)
            => async operation => await UpdateAsync(operation, dto, TabTypes.Bank);

        private Func<OperationResult<IEnumerable<ExamDetailsQuestionDto>>, Task<OperationResult<IEnumerable<ExamDetailsQuestionDto>>>> _getAllBankQuestion(Guid id)
            => async operation =>
            {
                var bankQuestions = await GetAllQuestionAsync(exam => exam.Id == id && exam.Type == TabTypes.Bank);
                return operation.SetSuccess(bankQuestions);
            };

        #endregion

        #region - Interview -

        private Func<OperationResult<IEnumerable<ExamDetailsDto>>, Task<OperationResult<IEnumerable<ExamDetailsDto>>>> _getAllInterview()
            => async operation =>
            {
                var Interviews = await GetAllAsync(exam => exam.Type == TabTypes.Interview);
                return operation.SetSuccess(Interviews);
            };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _deleteInterview(Guid id)
            => async operation =>
            {
                if (!(await TryDelete(id, TabTypes.Interview)))
                {
                    return (OperationResultTypes.NotExist, "${id} : not exist.");
                }
                return operation.SetSuccess(true, "Success Delete.");
            };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _multiDeleteInterview(IEnumerable<Guid> ids)
            => async operation =>
            {
                if (!(await TryMultiDelete(ids, TabTypes.Interview)))
                {
                    return (OperationResultTypes.NotExist, "${ids} : not exist.");
                }
                return operation.SetSuccess(true, "Success Delete.");
            };

        private Func<OperationResult<ExamDetailsDto>, Task<OperationResult<ExamDetailsDto>>> _addInterview(ExamDto dto)
            => async operation =>
            {
                var interview = await AddAsync(dto, TabTypes.Interview);
                return operation.SetSuccess(interview);
            };

        private Func<OperationResult<ExamDetailsDto>, Task<OperationResult<ExamDetailsDto>>> _updateInterview(ExamDto dto)
            => async operation => await UpdateAsync(operation, dto, TabTypes.Interview);

        private Func<OperationResult<IEnumerable<ExamDetailsQuestionDto>>, Task<OperationResult<IEnumerable<ExamDetailsQuestionDto>>>> _getAllInterviewQuestion(Guid id)
            => async operation =>
            {
                var interviewQuestions = await GetAllQuestionAsync(exam => exam.Id == id && exam.Type == TabTypes.Interview);
                return operation.SetSuccess(interviewQuestions);
            };

        #endregion

        #region - Microscope -

        private Func<OperationResult<IEnumerable<ExamDetailsDto>>, Task<OperationResult<IEnumerable<ExamDetailsDto>>>> _getAllMicroscope()
            => async operation =>
            {
                var Microscopes = await GetAllAsync(exam => exam.Type == TabTypes.Microscope);
                return operation.SetSuccess(Microscopes);
            };

        private Func<OperationResult<MicroscopeDocumentsDto>, Task<OperationResult<MicroscopeDocumentsDto>>> _detailsMicroscope(Guid id)
            => async operation =>
            {
                var microscope = await Query.Where(exam => exam.Id == id)
                                            .Select(exam => new MicroscopeDocumentsDto
                                            {
                                                Id = exam.Id,
                                                Name = exam.Name,
                                                IsFree = exam.IsFree,
                                                Price = exam.Price,
                                                SubjectId = exam.SubjectId,
                                                Year = exam.Year,
                                                Type = TabTypes.Microscope,

                                                TagIds = exam.ExamTags.Select(t => t.TagId).ToList(),

                                                Documents = exam.ExamDocuments.Select(doc => new ExamDocumentDto
                                                {
                                                    Id = doc.Id,
                                                    Note = doc.Note,
                                                    Path = doc.Document.Path,
                                                }).ToList(),

                                                Sections = exam.ExamQuestions.Select(section => new ExamQuestionDocumnetsDto
                                                {
                                                    Id = section.Id,
                                                    Order = section.Order,
                                                    Hint = section.Question.Hint,
                                                    Title = section.Question.Title,
                                                    IsCorrected = section.Question.IsCorrected,
                                                    DateCreated = section.DateCreated,
                                                    QuestionType = section.Question.QuestionType,
                                                    AnswerType = section.Question.AnswerType,
                                                    Tags = section.Question.QuestionTags.Select(t => t.TagId),
                                                    Documents = section.Question.QuestionDocuments.Select(doc => new QuestionDocumentNote
                                                    {
                                                        SectionImageId = doc.Id,
                                                        Note = doc.Note,
                                                        Path = doc.Document.Path,
                                                    }),
                                                }).ToList(),
                                            }).FirstOrDefaultAsync();
                return operation.SetSuccess(microscope);
            };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _deleteMicroscope(Guid id)
            => async operation =>
            {
                var microscope = await TrackingQuery.Where(exam => exam.Id == id)
                                                    .Include(exam => exam.ExamTags)
                                                    .Include(exam => exam.ExamQuestions)
                                                    .ThenInclude(q => q.Question)
                                                    .ThenInclude(q => q.Answers)
                                                    .Include("ExamQuestions.Question.QuestionTags")
                                                    .Include("ExamQuestions.Question.QuestionDocuments")
                                                    .Include("ExamQuestions.Question.QuestionDocuments.Document")
                                                    .SingleOrDefaultAsync();
                if (microscope == null)
                {
                    return (OperationResultTypes.NotExist, $"{id} : not exist.");
                }
                foreach (var tag in microscope.ExamTags)
                {
                    Context.SoftDelete(tag);
                }
                foreach (var question in microscope.ExamQuestions)
                {
                    foreach (var answer in question.Question.Answers)
                    {
                        Context.SoftDelete(answer);
                    }
                    foreach (var tag in question.Question.QuestionTags)
                    {
                        Context.SoftDelete(tag);
                    }
                    foreach (var doc in question.Question.QuestionDocuments)
                    {
                        if (doc.Document.QuestionDocuments.Where(e => e.QuestionId != question.QuestionId).Count() == 1)
                        {
                            await TryDeleteFileAsync(doc.Document.Path);
                            Context.Documents.Remove(doc.Document);
                        }
                    }
                    Context.SoftDelete(question);
                }
                Context.SoftDelete(microscope);
                await Context.SaveChangesAsync();
                return operation.SetSuccess(true);
            };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _multiDeleteMicroscope(IEnumerable<Guid> ids)
            => async operation =>
            {
                foreach (var id in ids)
                {
                    await DeleteMicroscope(id);
                }
                return operation.SetSuccess(true);
            };

        private Func<OperationResult<ExamDetailsDto>, Task<OperationResult<ExamDetailsDto>>> _addMicroscope(ExamDto dto)
            => async operation =>
            {
                var microscope = await AddAsync(dto, TabTypes.Microscope);
                return operation.SetSuccess(microscope);
            };

        private Func<OperationResult<ExamDetailsDto>, Task<OperationResult<ExamDetailsDto>>> _updateMicroscope(ExamDto dto)
            => async operation => await UpdateAsync(operation, dto, TabTypes.Microscope);

        private Func<OperationResult<MicroscopeDocumentsDto>, Task<OperationResult<MicroscopeDocumentsDto>>> _updateSectionsMicroscope(SectionsMicroscopeDocumentsDto dto)
            => async operation =>
            {
                var microscope = await TrackingQuery.Where(exam => exam.Id == dto.Id)
                                                    .Include(e => e.ExamQuestions)
                                                    .ThenInclude(e => e.Question)
                                                    .ThenInclude(q => q.QuestionTags)
                                                    .ThenInclude(q => q.Question.QuestionDocuments)
                                                    .ThenInclude(q => q.Document)
                                                    .FirstOrDefaultAsync();
                if (microscope == null)
                    return (OperationResultTypes.NotExist, $"{dto.Id} : Microscope not Found.");

                using (var transaction = await Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        foreach (var examQuestions in microscope.ExamQuestions)
                        {
                            var section = dto.Sections.SingleOrDefault(sec => sec.Id == examQuestions.Id);
                            if (section is not null)
                            {
                                examQuestions.Order = section.Order;
                                examQuestions.Question.Title = section.Title;
                                examQuestions.Question.Hint = section.Hint;
                                examQuestions.Question.IsCorrected = section.IsCorrected;
                                examQuestions.Question.QuestionType = section.QuestionType;
                                examQuestions.Question.AnswerType = section.AnswerType;
                                foreach (var docs in examQuestions.Question.QuestionDocuments)
                                {
                                    var doc = section.Documents.SingleOrDefault(d => d.SectionImageId == docs.Id);
                                    if (doc is not null)
                                        docs.Note = doc.Note;
                                }
                            }

                            (IEnumerable<Guid> newTags, IEnumerable<Guid> oldTags) = examQuestions.Question.QuestionTags.IsolatedExcept(section?.Tags ?? new List<Guid>(), x => x.TagId, x => x);
                            foreach (var oldTag in oldTags)
                            {
                                examQuestions.Question.QuestionTags.Remove(examQuestions.Question.QuestionTags.Single(t => t.TagId == oldTag));
                            }
                            var AddTag = section?.Tags.Where(tag => newTags.Contains(tag)).Select(tag => new QuestionTag { TagId = tag, QuestionId = section.Id });
                            foreach (var newTag in AddTag)
                            {
                                examQuestions.Question.QuestionTags.Add(newTag);
                            }

                            (IEnumerable<QuestionDocumentNote> newDocs, IEnumerable<QuestionDocument> oldDocs) = examQuestions.Question.QuestionDocuments.IsolatedExceptOldNew(section?.Documents ?? new List<QuestionDocumentNote>(), x => x.DocumentId, x => x.SectionImageId);
                            foreach (var oldDoc in oldDocs)
                            {
                                examQuestions.Question.QuestionDocuments.Remove(oldDoc);
                                var exams = Context.QuestionDocuments.Where(q => q.DocumentId == oldDoc.Id && q.DateDeleted == null).ToList();
                                if (!exams.Any())
                                {
                                    await TryDeleteFileAsync(oldDoc.Document.Path);
                                    Context.Documents.Remove(oldDoc.Document);
                                }

                            }

                            foreach (var newDoc in newDocs)
                            {
                                examQuestions.Question.QuestionDocuments.Add(new QuestionDocument
                                {
                                    Document = new Document()
                                    {
                                        Type = DocumentTypes.Image
                                    },
                                    Note = newDoc.Note,
                                });
                                dto.Sections.ToList().First().Documents.ToList().Add(new QuestionDocumentNote
                                {
                                    SectionImageId = newDoc.SectionImageId,
                                    Note = newDoc.Note,
                                    File = newDoc.File,
                                    Path = newDoc.Path
                                });
                            }
                        }
                        await Context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return operation.SetSuccess(new MicroscopeDocumentsDto() { Id = dto.Id, Sections = dto.Sections });
                    }
                    catch (Exception ex )
                    {
                        await transaction.RollbackAsync();
                        return operation.SetException(ex);
                    }
                }
            };


        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _deleteSectionsMicroscope(Guid id)
            => async operation =>
            {
                
                using(var transaction = await Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var microscope = await _trackingQuery<Question>().Where(q => q.Id == id).Include(q => q.QuestionDocuments).ThenInclude(d => d.Document).SingleOrDefaultAsync();
                        if (microscope == null)
                            return (OperationResultTypes.NotExist, $"{id} : not exist.");
                        Context.SoftDelete(microscope);
                        foreach (var doc in microscope.QuestionDocuments)
                        {
                            Context.SoftDelete(doc);

                            if (!doc.Document.QuestionDocuments.Where(e => e.QuestionId != microscope.Id).Any())
                            {
                                await TryDeleteFileAsync(doc.Document.Path);
                                Context.Documents.Remove(doc.Document);
                            }
                            Context.QuestionDocuments.Remove(doc);
                        }

                        await Context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return operation.SetSuccess(true);
                    
                    }
                    catch(Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return operation.SetException(ex);
                    }
                }
            };

        #endregion

        #region - Tab -

        private async Task<IEnumerable<ExamDetailsDto>> GetAllAsync(Expression<Func<Exam, bool>> predicate)
        {
            return await Query.Where(exam => exam.Type == TabTypes.Exam)
                              .Include(exam => exam.Subject)
                              .Select(exam => new ExamDetailsDto
                              {
                                  Id = exam.Id,
                                  Name = exam.Name,
                                  Year = exam.Year,
                                  Type = exam.Type,
                                  DateCreated = exam.DateCreated,
                                  SubjectId = exam.SubjectId,
                                  SubjectName = exam.Subject.Name,
                                  Price = exam.Price,
                                  IsFree = exam.IsFree,
                                  QuestionsCount = exam.ExamQuestions.Count(),
                                  SemesterId = exam.Subject.Faculties.Select(f => f.SemesterId).ToList(),
                                  SectionId = exam.Subject.Faculties.Select(f => f.SectionId).ToList(),
                                  TagIds = exam.ExamTags.Select(et => et.Id),
                              }).ToListAsync();
        }

        private async Task<bool> TryDelete(Guid examId, TabTypes examType)
        {
            var one = await TrackingQuery.Where(exam => exam.Id == examId).Include(e => e.ExamQuestions).Include(e => e.ExamTags).Include(e => e.ExamDocuments).FirstOrDefaultAsync();
            if (one is null)
                return false;
            foreach (var tag in one.ExamTags)
            {
                Context.SoftDelete(tag);
            }
            foreach (var question in one.ExamQuestions)
            {
                Context.SoftDelete(question);
            }
            foreach (var doc in one.ExamDocuments)
            {
                var exams = Context.ExamDocuments.Where(ed => ed.DocumentId == doc.Id && ed.DateDeleted == null).ToList();
                if(exams.Count() == 1)
                {
                    await TryDeleteFileAsync(doc.Document.Path);
                    Context.Documents.Remove(doc.Document);
                }
            }
            Context.SoftDelete(one);
            await Context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> TryMultiDelete(IEnumerable<Guid> examIds, TabTypes examType)
        {
            var list = await TrackingQuery.Where(exam => examIds.Contains(exam.Id)).Include(e => e.ExamQuestions).Include(e => e.ExamTags).Include(e => e.ExamDocuments).ToListAsync();
            if (list is null)
                return false;
            foreach (var one in list)
            {
                foreach (var tag in one.ExamTags)
                {
                    Context.SoftDelete(tag);
                }
                foreach (var question in one.ExamQuestions)
                {
                    Context.SoftDelete(question);
                }
                var docs = Context.ExamDocuments.Where(ed => ed.ExamId == one.Id).ToList();
                foreach (var doc in docs)
                {
                    var exams = Context.ExamDocuments.Where(ed => ed.DocumentId == doc.Id && ed.DateDeleted == null).ToList();
                    if (exams.Count() == 1)
                    {
                        await TryDeleteFileAsync(doc.Document.Path);
                        Context.Documents.Remove(doc.Document);
                    }
                }
            }
            list.ForEach(item => Context.SoftDelete(item));
            await Context.SaveChangesAsync();
            return true;
        }

        private async Task<ExamDetailsDto> AddAsync(ExamDto dto, TabTypes examType)
        {
            var exam = new Exam
            {
                Name = dto.Name,
                IsFree = dto.IsFree,
                Price = dto.Price,
                Year = dto.Year,
                SubjectId = dto.SubjectId,
                Type = examType,
            };

            if(dto.TagIds is not null)
            {
                exam.ExamTags = dto.TagIds.Distinct().Select(id => new ExamTag
                {
                    TagId = id
                }).ToList();
            }

            await Context.AddAsync(exam);
            await Context.SaveChangesAsync();
            dto.Id = exam.Id;

            return await Query.Where(exam => exam.Id == dto.Id)
                              .Select(exam => new ExamDetailsDto
                              {
                                  Id = exam.Id,
                                  Name = exam.Name,
                                  Year = exam.Year,
                                  DateCreated = exam.DateCreated,
                                  IsFree = exam.IsFree,
                                  Price = exam.Price,
                                  Type = exam.Type,
                                  TagIds = exam.ExamTags.Select(et => et.TagId),
                                  SubjectName = exam.Subject.Name,
                                  SubjectId = exam.SubjectId,
                                  SemesterId = exam.Subject.Faculties.Select(f => f.SemesterId).ToList(),
                                  SectionId = exam.Subject.Faculties.Select(f => f.SectionId).ToList(),
                                  QuestionsCount = exam.ExamQuestions.Count(),
                              }).FirstOrDefaultAsync();

        }

        private async Task<OperationResult<ExamDetailsDto>> UpdateAsync(OperationResult<ExamDetailsDto> operation, ExamDto dto, TabTypes examType)
        {
            var exam = await FindAsync(dto.Id);

            if (exam is null || exam.Type != examType)
                return OperationResultTypes.NotExist;
            exam.Name = dto.Name;
            exam.IsFree = dto.IsFree;
            exam.Price = dto.Price;
            exam.SubjectId = dto.SubjectId;
            exam.Year = dto.Year;
            var currentTags = await Context.ExamTags.Where(et => et.ExamId == dto.Id).ToListAsync();
            var newTags = dto.TagIds.Except(currentTags.Select(et => et.Id));
            var oldTags = currentTags.Select(et => et.Id).Except(dto.TagIds).ToList();
            Context.ExamTags.RemoveRange(currentTags.Where(t => oldTags.Contains(t.Id)).ToList());
            await Context.ExamTags.AddRangeAsync(newTags.Select(et => new ExamTag() { ExamId = dto.Id, TagId = et }));
            await Context.SaveChangesAsync();
            return operation.SetSuccess(await Query.Where(exam => exam.Id == dto.Id)
                            .Select(exam => new ExamDetailsDto
                            {
                                Id = exam.Id,
                                Name = exam.Name,
                                Year = exam.Year,
                                DateCreated = exam.DateCreated,
                                IsFree = exam.IsFree,
                                Price = exam.Price,
                                Type = exam.Type,
                                TagIds = exam.ExamTags.Select(et => et.TagId),
                                SubjectName = exam.Subject.Name,
                                SubjectId = exam.SubjectId,
                                SemesterId = exam.Subject.Faculties.Select(f => f.SemesterId).ToList(),
                                SectionId = exam.Subject.Faculties.Select(f => f.SectionId).ToList(),
                                QuestionsCount = exam.ExamQuestions.Count(e => e.DateDeleted == null),
                            }).FirstOrDefaultAsync());
        }

        private async Task<IEnumerable<ExamDetailsQuestionDto>> GetAllQuestionAsync(Expression<Func<Exam, bool>> predicate)
        {
            return await Query.Where(predicate)
                              .Select(exam => new ExamDetailsQuestionDto
                              {
                                  Id = exam.Id,
                                  Name = exam.Name,
                                  Year = exam.Year,
                                  Type = exam.Type,
                                  DateCreated = exam.DateCreated,
                                  SubjectId = exam.SubjectId,
                                  Price = exam.Price,
                                  IsFree = exam.IsFree,
                                  QuestionsCount = exam.ExamQuestions.Count(),
                                  SemesterId = exam.Subject.Faculties.Select(f => f.SemesterId).ToList(),
                                  SectionId = exam.Subject.Faculties.Select(f => f.SectionId).ToList(),
                                  TagIds = exam.ExamTags.Select(et => et.Id),
                                  Questions = exam.ExamQuestions.Select(question => new QuestionImagesTagsAnswersDto
                                  {
                                      Id = question.QuestionId,
                                      Title = question.Question.Title,
                                      Hint = question.Question.Hint,
                                      Order = question.Order,
                                      QuestionType = question.Question.QuestionType,
                                      AnswerType = question.Question.AnswerType,
                                      DateCreated = question.Question.DateCreated,
                                      IsCorrected = question.Question.IsCorrected,
                                      Tags = question.Question.QuestionTags.Select(q => q.TagId),
                                      Images = question.Question.QuestionDocuments.Select(questiondoc => new QuestionDocumentNote
                                      {
                                          SectionImageId = questiondoc.QuestionId,
                                          Note = questiondoc.Note,
                                          Path = questiondoc.Document.Path,
                                      }),
                                      Answers = question.Question.Answers.Select(answer => new AnswerDto
                                      {
                                          Id = answer.Id,
                                          Title = answer.Title,
                                          IsCorrect = answer.IsCorrect,
                                          CorrectionDate = answer.CorrectionDate,
                                          QuestionId = answer.QuestionId,
                                          CorrectAnswerId = answer.CorrectAnswerId
                                      }),
                                  }),
                                  Documents = exam.ExamDocuments.Select(doc => new ExamDocumentDto
                                  {
                                      Id = doc.Id,
                                      Note = doc.Note,
                                      Path = doc.Document.Path
                                  }),
                              }).ToListAsync();
        }

        private async Task<OperationResult<bool>> TryDeleteFileAsync(string path)
        {
            if(!path.IsNullOrEmpty())
            {
                var pathFile = Path.Combine(webHostEnvironment.WebRootPath, path);
                try
                {
                    File.Delete(path);
                }
                catch(Exception ex)
                {
                    return new OperationResult<bool>().SetException(ex);
                }
            }
            return new OperationResult<bool>().SetSuccess(true);
        }
        #endregion

    }
}
