using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.ExamDto;
using SmartStart.DataTransferObject.QuestionDto;
using SmartStart.Model.Main;
using SmartStart.SharedKernel.Enums;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Main.ExamServices
{
    [ElRepository]
    public class ExamRepository : ElRepository<SmartStartDbContext, Guid, Exam>, IExamRepository
    {
        public ExamRepository(SmartStartDbContext context) : base(context)
        {
        }


        public async Task<OperationResult<IEnumerable<ExamDetailsDto>>> GetAllExam()
            => await RepositoryHandler(_getAllExam());

        public async Task<OperationResult<bool>> DeleteExam(Guid id)
            => await RepositoryHandler(_deleteExam(id));

        public async Task<OperationResult<bool>> MultiDeleteExam(IEnumerable<Guid> ids)
            => await RepositoryHandler(_multiDelete(ids));

        public async Task<OperationResult<ExamDetailsDto>> AddExam(ExamDto exam)
            => await RepositoryHandler(_addExam(exam));

        public async Task<OperationResult<ExamDetailsDto>> UpdateExam(ExamDto dto)
            => await RepositoryHandler(_updateExam(dto));

        public async Task<OperationResult<IEnumerable<ExamDetailsQuestionDto>>> GetAllExamQuestion(Guid id)
            => await RepositoryHandler(_getAllExamQuestion(id));

        
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

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _multiDelete(IEnumerable<Guid> ids)
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

        #region Tab

        private async Task<IEnumerable<ExamDetailsDto>> GetAllAsync(Expression<Func<Exam, bool>> predicate)
        {
            return await Query.Where(exam => exam.Type == TabTypes.Exam)
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
                                  SemesterId = exam.Subject.SubjectTags.Where(s => s.Tag.Type == TagTypes.Semester).Select(s => s.Id).FirstOrDefault(),
                                  SectionId = exam.Subject.SubjectTags.Where(s => s.Tag.Type == TagTypes.Section).Select(s => s.Id).FirstOrDefault(),
                                  TagIds = exam.ExamTags.Select(et => et.Id),
                              }).ToListAsync();
        }

        private async Task<bool> TryDelete(Guid examId, TabTypes examType)
        {
            var one = await TrackingQuery.Where(exam => exam.Id == examId).Include(e => e.ExamQuestions).Include(e => e.ExamTags).Include(e => e.ExamDocuments).FirstOrDefaultAsync();
            if (one is null)
                return false;
            //var docIds = Context.ExamDocument
            Context.SoftDelete(one);
            await Context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> TryMultiDelete(IEnumerable<Guid> examIds, TabTypes examType)
        {
            var list = await TrackingQuery.Where(exam => examIds.Contains(exam.Id)).Include(e => e.ExamQuestions).Include(e => e.ExamTags).ToListAsync();
            if (list is null)
                return false;
            list.ForEach(e => Context.SoftDelete(e));
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

            await SmartStart.SqlServer.DataBase.Seed.DataSeed.FixPackages(Context);
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
                                  SemesterId = exam.Subject.SubjectTags.Where(s => s.Tag.Type == TagTypes.Semester)
                                                                       .Select(s => s.TagId).FirstOrDefault(),
                                  SectionId = exam.Subject.SubjectTags.Where(s => s.Tag.Type == TagTypes.Section)
                                                                      .Select(s => s.Id).FirstOrDefault(),
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
                                SemesterId = exam.Subject.SubjectTags.Where(s => s.Tag.Type == TagTypes.Semester)
                                                                     .Select(s => s.TagId).FirstOrDefault(),
                                SectionId = exam.Subject.SubjectTags.Where(s => s.Tag.Type == TagTypes.Section)
                                                                    .Select(s => s.Id).FirstOrDefault(),
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
                                  SemesterId = exam.Subject.SubjectTags.Where(st => st.Tag.Type == TagTypes.Semester)
                                                                       .Select(st => st.Id).FirstOrDefault(),
                                  SectionId = exam.Subject.SubjectTags.Where(s => s.Tag.Type == TagTypes.Section)
                                                                      .Select(s => s.Id).FirstOrDefault(),
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

        
        #endregion

    }
}
