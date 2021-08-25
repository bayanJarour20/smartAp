using Elkood.Web.Common.ContextResult.OperationContext;
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

        public ExamRepository(SmartStartDbContext context, IWebHostEnvironment webHostEnvironment) : base(context) { }

        #region Exam
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
        #endregion

        #region Bank
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

        #endregion

        #region Interview
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

        #endregion

        #region Microscope
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
        #endregion

        #region ExamDocumnets
        public async Task<OperationResult<ExamDocumentDto>> AddExamDocument(ExamDocumentDto dto)
           => await RepositoryHandler(_addExamDocument(dto));
        public async Task<OperationResult<bool>> DeleteExamDocument(Guid documentId)
            => await RepositoryHandler(_deleteExamDocument(documentId));
        public async Task<OperationResult<bool>> DeleteExamDocument(IEnumerable<Guid> documentIds)
            => await RepositoryHandler(_deleteRangeExamDocuments(documentIds));
        #endregion

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
                if (!(await TryDeleteAsync(id, TabTypes.Exam)))
                    return (OperationResultTypes.NotExist, $"{id} : not exist.");
                return operation.SetSuccess(true, " Success Delete. ");
            };
        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _multiDeleteExam(IEnumerable<Guid> ids)
            => async operation =>
            {
                if (!(await TryMultiDeleteAsync(ids, TabTypes.Exam)))
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
                var examQuestions = await GetAllQuestionAsync(exam => exam.Id == id 
                                                                   && exam.Type == TabTypes.Exam);
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
                if (!(await TryDeleteAsync(id, TabTypes.Bank)))
                    return (OperationResultTypes.NotExist, $"{id}: not exist. ");
                return operation.SetSuccess(true, "Sussecc Delete.");
            };
        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _multiDeleteBank(IEnumerable<Guid> ids)
            => async operation =>
            {
                if (!(await TryMultiDeleteAsync(ids, TabTypes.Bank)))
                    return (OperationResultTypes.NotExist, $"{ids}: not exist. ");
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
                if (!(await TryDeleteAsync(id, TabTypes.Interview)))
                    return (OperationResultTypes.NotExist, "${id} : not exist.");
                return operation.SetSuccess(true, "Success Delete.");
            };
        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _multiDeleteInterview(IEnumerable<Guid> ids)
            => async operation =>
            {
                if (!(await TryMultiDeleteAsync(ids, TabTypes.Interview)))
                    return (OperationResultTypes.NotExist, "${ids} : not exist.");
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
                if (!(await TryDeleteAsync(id, TabTypes.Microscope)))
                    return (OperationResultTypes.NotExist, "${id} : not exist.");
                return operation.SetSuccess(true, "Success Delete.");
            };
        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _multiDeleteMicroscope(IEnumerable<Guid> ids)
            => async operation =>
            {
                if (!(await TryMultiDeleteAsync(ids, TabTypes.Interview)))
                    return (OperationResultTypes.NotExist, "${ids} : not exist.");
                return operation.SetSuccess(true, "Success Delete.");
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
                                    var doc = section.Documents.Where(d => d.SectionImageId == docs.Id).SingleOrDefault();
                                    if (doc is not null)
                                        docs.Note = doc.Note;
                                }
                            }

                            (IEnumerable<Guid> newTags, IEnumerable<Guid> oldTags) = examQuestions.Question.QuestionTags.IsolatedExcept(section?.Tags ?? new List<Guid>(), x => x.TagId, x => x);
                            foreach (var oldTag in oldTags)
                                examQuestions.Question.QuestionTags.Remove(examQuestions.Question.QuestionTags.Single(t => t.TagId == oldTag));
                            
                            var AddTag = section?.Tags.Where(tag => newTags.Contains(tag)).Select(tag => new QuestionTag { TagId = tag, QuestionId = section.Id });
                            foreach (var newTag in AddTag)
                                examQuestions.Question.QuestionTags.Add(newTag);

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
                                    Note = newDoc.Note,
                                    Document = new Document
                                    {
                                        Type = DocumentTypes.Image
                                    },
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
                        return operation.SetSuccess(new MicroscopeDocumentsDto { Id = dto.Id, Sections = dto.Sections });
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
                    var microscope = await _query<ExamQuestion>().Where(q => q.Id == id).SingleOrDefaultAsync();
                    if (microscope == null)
                        return (OperationResultTypes.NotExist, $"{id} : not exist.");
                    Context.SoftDelete(microscope);

                    await Context.SaveChangesAsync();
                    return operation.SetSuccess(true);
            };
        #endregion

        #region - Tab -
        private async Task<IEnumerable<ExamDetailsDto>> GetAllAsync(Expression<Func<Exam, bool>> predicate)
        {
            return await Query.Where(predicate)
                              .Include(exam => exam.Subject)
                              .Select(exam => fileExamDetails(exam)).ToListAsync();
        }
        private async Task<bool> TryDeleteAsync(Guid examId, TabTypes examType)
        {
            var one = await TrackingQuery.Where(exam => exam.Id == examId)
                                         .Include(e => e.ExamQuestions)
                                         .Include(e => e.ExamTags)
                                         .Include(e => e.ExamDocuments)
                                         .FirstOrDefaultAsync();
            if (one is null)
                return false;
            Context.SoftDelete(one);
            Delete(one);
            await Context.SaveChangesAsync();
            return true;
        }
        private async Task<bool> TryMultiDeleteAsync(IEnumerable<Guid> examIds, TabTypes examType)
        {
            var list = await TrackingQuery.Where(exam => examIds.Contains(exam.Id))
                                          .Include(e => e.ExamQuestions)
                                          .Include(e => e.ExamTags)
                                          .Include(e => e.ExamDocuments)
                                          .ToListAsync();
            if (list is null)
                return false;
            list.ForEach(item => Context.SoftDelete(item));

            foreach (var one in list)
                Delete(one);

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
                exam.ExamTags = dto.TagIds.Distinct().Select(id => new ExamTag { TagId = id }).ToList();
            }
            await Context.AddAsync(exam);
            await Context.SaveChangesAsync();
            dto.Id = exam.Id;
            return await Query.Where(exam => exam.Id == dto.Id)
                              .Select(exam => fileExamDetails(exam)).FirstOrDefaultAsync();
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

            (IEnumerable<Guid> addTag, IEnumerable<ExamTag> removeTag) = exam.ExamTags.IsolatedExceptOldNew(dto?.TagIds ?? new List<Guid>(), x => x.TagId, x => x);
            Context.RemoveRange(removeTag);
            await Context.ExamTags.AddRangeAsync(addTag.Select(tag => new ExamTag() { ExamId = dto.Id, TagId = tag }));
            await Context.SaveChangesAsync();

            return operation.SetSuccess(await Query.Where(exam => exam.Id == dto.Id)
                            .Select(exam => fileExamDetails(exam)).FirstOrDefaultAsync());
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
                                  SemesterId = exam.Subject.SubjectFaculties.Where(e => e.SemesterId.HasValue).Select(f => f.SemesterId.Value),
                                  SectionId = exam.Subject.SubjectFaculties.Where(e => e.SectionId.HasValue).Select(f => f.SectionId.Value),
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
        private static ExamDetailsDto fileExamDetails(Exam exam)
        {
            return new ExamDetailsDto
            {
                Id = exam.Id,
                Name = exam.Name,
                Year = exam.Year,
                Type = exam.Type,
                IsFree = exam.IsFree,
                DateCreated = exam.DateCreated,
                SubjectId = exam.SubjectId,
                SubjectName = exam.Subject.Name,
                Price = exam.Price,
                TagIds = exam.ExamTags.Select(et => et.Id),
                SemesterId = exam.Subject.SubjectFaculties.Where(e => e.SemesterId.HasValue).Select(f => f.SemesterId.Value),
                SectionId = exam.Subject.SubjectFaculties.Where(e => e.SectionId.HasValue).Select(f => f.SectionId.Value),
                QuestionsCount = exam.ExamQuestions.Count(),
            };
        }
        private async void Delete(Exam exam)
        {
            foreach (var tag in exam.ExamTags)
                Context.SoftDelete(tag);

            foreach (var question in exam.ExamQuestions)
                Context.SoftDelete(question);

            foreach (var doc in exam.ExamDocuments)
            {
                var exams = Context.ExamDocuments.Where(ed => ed.DocumentId == doc.Id
                                                           && ed.DateDeleted == null).ToList();
                if (!exams.Any())
                {
                    await TryDeleteFileAsync(doc.Document.Path);
                    Context.Documents.Remove(doc.Document);
                }
            }
        }
        #endregion

        #region - ExamDocumnets - 
        private Func<OperationResult<ExamDocumentDto>, Task<OperationResult<ExamDocumentDto>>> _addExamDocument(ExamDocumentDto examDocumentDto)
            => async operation => {

                var result = TryUploadImage(examDocumentDto.File, out string path);
                if (result.IsSuccess)
                {
                    Document documentModel = new Document()
                    {
                        Name = examDocumentDto.Name,
                        Path = path,
                        Type = examDocumentDto.Type,
                    };

                    ExamDocument examDocumentModel = new ExamDocument()
                    {
                        Document = documentModel,
                        ExamId = examDocumentDto.Id,
                        Note = examDocumentDto.Note
                    };

                    await Context.ExamDocuments.AddAsync(examDocumentModel);

                    await Context.SaveChangesAsync();

                    examDocumentDto.Path = documentModel.Path;

                    return operation.SetSuccess(examDocumentDto);
                }
                return operation.SetFailed("Failed upload, Message: " + result.FullExceptionMessage);

            };
        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _deleteExamDocument(Guid documentId)
            => async operation =>
            {

                await Context.SoftDeleteTraversalAsync<Document, ExamDocument>(p => p.Id == documentId, p => p.ExamDocuments);

                var documentPath = await Context.Documents.Where(e => e.DateDeleted == null)
                                                    .Where(e => e.Id == documentId)
                                                    .Select(e => e.Path)
                                                    .FirstOrDefaultAsync();

                TryDeleteImage(documentPath);

                //Context.SoftDelete(await FindAsync(id));

                return operation.SetSuccess(true);
            };
        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _deleteRangeExamDocuments(IEnumerable<Guid> documentIds)
          => async operation =>
          {

              foreach (Guid id in documentIds)
              {
                  await Context.SoftDeleteTraversalAsync<Document, ExamDocument>(p => p.Id == id, p => p.ExamDocuments);

                  var documentPath = await Context.Documents.Where(e => e.DateDeleted == null)
                                                  .Where(e => e.Id == id)
                                                  .Select(e => e.Path)
                                                  .FirstOrDefaultAsync();
                  if (documentPath != null)
                      TryDeleteImage(documentPath);

              }

                //Context.SoftDelete(await FindAsync(id));

                return operation.SetSuccess(true);
          };
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


        #region Helper Functions

        private OperationResult<bool> TryUploadImage(IFormFile image, out string path)
        {
            path = null;
            try
            {
                if (image != null)
                {
                    var documentsDirectory = Path.Combine("wwwroot", "Documents", "Exam_Document");
                    if (!Directory.Exists(documentsDirectory))
                    {
                        Directory.CreateDirectory(documentsDirectory);
                    }
                    path = Path.Combine("Documents", "Exam_Document", Guid.NewGuid().ToString() + "_" + image.FileName);
                    string filePath = Path.Combine(webHostEnvironment.WebRootPath, path);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }
                }
                return new OperationResult<bool>().SetSuccess(true);
            }
            catch (Exception e) when (e is IOException || e is Exception)
            {
                return new OperationResult<bool>().SetException(e);
            }
        }

        private OperationResult<bool> TryDeleteImage(string path)
        {
            if (!path.IsNullOrEmpty())
            {
                string filePath = Path.Combine(webHostEnvironment.WebRootPath, path);
                try { File.Delete(filePath); } catch (Exception e) { return new OperationResult<bool>().SetException(e); }
            }
            return new OperationResult<bool>().SetSuccess(true);
        }

        #endregion
    }
}
