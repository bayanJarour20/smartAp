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

namespace SmartStart.Repository.Main.QuestionServices
{
    [ElRepository]
    public class QuestionRepository : ElRepository<SmartStartDbContext, Guid, Question>, IQuestionRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public QuestionRepository(SmartStartDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<OperationResult<IEnumerable<QuestionTagsAnswersDto>>> GetAll(int? year, Guid? semesterId, Guid? subjectId)
            => await RepositoryHandler(_getAll(year, semesterId, subjectId));
        public async Task<OperationResult<QuestionTagsAnswersExamsDto>> Add(QuestionTagsAnswersExamsDto question)
            => await RepositoryHandler(_add(question));
        public async Task<OperationResult<QuestionTagsAnswersExamsDto>> Update(QuestionTagsAnswersExamsDto question)
            => await RepositoryHandler(_update(question));
        public async Task<OperationResult<QuestionTagsAnswersExamsDto>> Details(Guid id)
            => await RepositoryHandler(_details(id));
        public async Task<OperationResult<bool>> Delete(Guid id)
            => await RepositoryHandler(_delete(id));
        public async Task<OperationResult<QuestionAnswersDto>> Correct(QuestionAnswersDto question)
        => await RepositoryHandler(_correctQuestion(question));



        private Func<OperationResult<IEnumerable<QuestionTagsAnswersDto>>, Task<OperationResult<IEnumerable<QuestionTagsAnswersDto>>>> _getAll(int? year, Guid? semesterId, Guid? subjectId)
            => async operation =>
            {
                var list = await Query.Where(question => (!year.HasValue || question.ExamQuestions.Any(q => q.Exam.Year == year))
                                                      && (!semesterId.HasValue || question.ExamQuestions.Any(q => q.Exam.ExamTags.Any(e => e.TagId == semesterId)))
                                                      && (!subjectId.HasValue || question.ExamQuestions.Any(q => q.Exam.SubjectId == subjectId)))
                                      .Select(question => new QuestionTagsAnswersDto
                                      {
                                          Id = question.Id,
                                          Title = question.Title,
                                          Hint = question.Hint,
                                          QuestionType = question.QuestionType,
                                          AnswerType = question.AnswerType,
                                          Tags = question.QuestionTags.Select(qt => qt.TagId),
                                          DateCreated = question.DateCreated,
                                          IsCorrected = question.IsCorrected,
                                          Answers = question.Answers.Where(a => !a.CorrectionDate.HasValue).Select(a => new AnswerDto
                                          {
                                              Id = a.Id,
                                              Title = a.Title,
                                              IsCorrect = a.IsCorrect,
                                              CorrectionDate = a.CorrectionDate,
                                              CorrectAnswerId = a.CorrectAnswerId,
                                              QuestionId = a.QuestionId,
                                          }),
                                      }).ToListAsync();
                return operation.SetSuccess(list);
            };
        private Func<OperationResult<QuestionTagsAnswersExamsDto>, Task<OperationResult<QuestionTagsAnswersExamsDto>>> _add(QuestionTagsAnswersExamsDto question)
            => async operation =>
            {

                var data = new Question()
                {
                    Title = question.Title,
                    Hint = question.Hint,
                    AnswerType = question.AnswerType,
                    QuestionType = question.QuestionType,
                    IsCorrected = question.IsCorrected,
                    QuestionTags = question.Tags?.Select(tag => new QuestionTag() { TagId = tag }).ToList(),
                    ExamQuestions = question.Exams?.Select(exam => new ExamQuestion() { Order = exam.Order, ExamId = exam.ExamId }).ToList(),
                    QuestionDocuments = (question.Images ??= new List<QuestionDocumentNote>()).Select(doc => new QuestionDocument()
                    {
                        Note = doc.Note,
                        Document = new Document()
                        {
                            Name = doc.File?.FileName,
                            Type = DocumentTypes.Image,
                        }
                    }).ToList(),
                    Answers = question.Answers?.Select(answer => new Answer()
                    {
                        Title = answer.Title,
                        IsCorrect = answer.IsCorrect,
                    }).ToList(),
                };

                List<string> savedPaths = new();
                foreach (var item in data.QuestionDocuments.Zip(question.Images))
                {
                    var result = (await TryUploadImageAsync(item.Second.File));
                    if (result.Result)
                    {
                        item.First.Document.Path = result.Message;
                        item.Second.Path = result.Message;
                        item.Second.File = null;//reset
                        savedPaths.Add(result.Message);
                    }
                    else
                    {
                        savedPaths.ForEach(d => TryDeleteImage(d));
                        return operation.SetFailed("Faild to save images");
                    }
                }

                await Context.AddAsync(data);
                await Context.SaveChangesAsync();

                question.Id = data.Id;
                question.DateCreated = data.DateCreated;
                question.Answers = data.Answers?.Select(a => new AnswerDto()
                {
                    Id = a.Id,
                    CorrectAnswerId = a.CorrectAnswerId,
                    CorrectionDate = a.CorrectionDate,
                    QuestionId = a.QuestionId,
                    Title = a.Title,
                    IsCorrect = a.IsCorrect,
                });
                question.Exams = data.ExamQuestions?.Select(e => new QuestionExamDto()
                {
                    Order = e.Order,
                    ExamId = e.ExamId,
                    QuestionId = e.QuestionId,
                    DateCreated = e.DateCreated,
                    ExamName = e.Exam?.Name
                });
                question.Images = data.QuestionDocuments.Select(e => new QuestionDocumentNote()
                {
                    Note = e.Note,
                    SectionImageId = e.Id,
                    Path = e.Document.Path,
                });

                return operation.SetSuccess(question);
            };
        private Func<OperationResult<QuestionTagsAnswersExamsDto>, Task<OperationResult<QuestionTagsAnswersExamsDto>>> _update(QuestionTagsAnswersExamsDto dto)
            => async operation =>
            {
                await Context.Answers.Include(ans => ans.CorrectAnswer).LoadAsync();
                var one = await TrackingQuery.Include(question => question.ExamQuestions)
                                             .Include(question => question.QuestionTags)
                                             .Include(question => question.Answers)
                                             .Include(x => x.QuestionDocuments)
                                             .ThenInclude(x => x.Document)
                                             .FirstOrDefaultAsync(question => question.Id == dto.Id);

                one.Title = dto.Title;
                one.Hint = dto.Hint;
                one.AnswerType = dto.AnswerType;
                one.QuestionType = dto.QuestionType;
                one.IsCorrected = dto.IsCorrected;

                List<string> savedPaths = new();

                // update exist note
                foreach (var (fromData, fromNew) in from fromData in one.QuestionDocuments
                                                    from fromNew in dto.Images ??= new List<QuestionDocumentNote>()
                                                    where fromData.Id == fromNew.SectionImageId
                                                    select (fromData, fromNew))
                fromData.Note = fromNew.Note;

                (IEnumerable<QuestionDocumentNote> AddDoc, IEnumerable<QuestionDocument> removeDoc) = one.QuestionDocuments.IsolatedExceptOldNew(dto.Images ??= new List<QuestionDocumentNote>(), x => x.Id, x => x.SectionImageId);

                foreach (var doc in removeDoc)
                {
                    one.QuestionDocuments.Remove(doc);
                    if (TryDeleteImage(doc.Document.Path).Result)
                        Context.Remove(doc.Document);
                }

                foreach (var doc in AddDoc)
                {
                    var initNewDoc = new QuestionDocument()
                    {
                        Note = doc.Note,
                        Document = new Document()
                        {
                            Name = doc.File?.FileName,
                            Type = DocumentTypes.Image,
                        }
                    };

                    var result = (await TryUploadImageAsync(doc.File));
                    if (result.Result)
                    {
                        initNewDoc.Document.Path = result.Message;
                        doc.Path = result.Message;
                        doc.File = null;
                        savedPaths.Add(result.Message);
                    }
                    else
                    {
                        savedPaths.ForEach(d => TryDeleteImage(d));
                        return operation.SetFailed("Faild to save docs");
                    }
                    one.QuestionDocuments.Add(initNewDoc);
                }

                // tags
                if (one.QuestionTags != null && one.QuestionTags.Any())
                {
                    (IEnumerable<Guid> AddTags, IEnumerable<Guid> removeTags) = one.QuestionTags.IsolatedExcept(dto.Tags, o => o.TagId, n => n);

                    foreach (var tag in removeTags)
                        one.QuestionTags.Remove(one.QuestionTags.First(t => t.TagId == tag));

                    foreach (var tag in dto.Tags?.Where(t => AddTags.Contains(t))
                                                 .Select(tag => new QuestionTag() { TagId = tag, QuestionId = dto.Id }))
                    one.QuestionTags.Add(tag);
                }

                // exams
                foreach (var item in one.ExamQuestions)
                {
                    var once = dto.Exams.SingleOrDefault(x => x.ExamId == item.ExamId);
                    if (once is not null)
                    {
                        item.Order = once.Order;
                    }
                }

                (IEnumerable<Guid> AddExams, IEnumerable<Guid> removeExams) = one.ExamQuestions.IsolatedExcept(dto.Exams, o => o.ExamId, n => n.ExamId);

                foreach (var exam in removeExams)
                    one.ExamQuestions.Remove(one.ExamQuestions.First(t => t.ExamId == exam));

                foreach (var exam in dto.Exams?.Where(t => AddExams.Contains(t.ExamId)).Select(exam => new ExamQuestion() { Order = exam.Order, ExamId = exam.ExamId, QuestionId = dto.Id }))
                    one.ExamQuestions.Add(exam);

                // asnwers
                foreach (var item in one.Answers)
                {
                    var once = dto.Answers.SingleOrDefault(x => x.Id == item.Id);
                    if (once is not null)
                    {
                        item.IsCorrect = once.IsCorrect;
                        item.Title = once.Title;
                    }
                }

                (IEnumerable<AnswerDto> AddAnswers, IEnumerable<Answer> removeAnswers) = one.Answers.IsolatedExceptOldNew(dto.Answers, o => o.Id, n => n.Id);

                foreach (var answer in removeAnswers)
                {
                    one.Answers.Remove(answer);
                    var temp = answer;
                    while (temp.CorrectAnswerId != null)
                    {
                        temp = temp.CorrectAnswer;
                        Context.Remove(temp);
                    }
                }

                foreach (var answer in AddAnswers)
                {
                    one.Answers.Add(new Answer()
                    {
                        QuestionId = dto.Id,
                        Title = answer.Title,
                        IsCorrect = answer.IsCorrect,
                    });
                }

                await Context.SaveChangesAsync();

                dto.Answers = one.Answers?.Select(a => new AnswerDto()
                {
                    Id = a.Id,
                    CorrectAnswerId = a.CorrectAnswerId,
                    CorrectionDate = a.CorrectionDate,
                    QuestionId = a.QuestionId,
                    Title = a.Title,
                    IsCorrect = a.IsCorrect,
                });

                dto.Exams = one.ExamQuestions.Select(e => new QuestionExamDto()
                {
                    Order = e.Order,
                    ExamId = e.ExamId,
                    QuestionId = e.QuestionId,
                    DateCreated = e.DateCreated,
                    ExamName = e.Exam?.Name
                });

                dto.Images = one.QuestionDocuments.Select(e => new QuestionDocumentNote()
                {
                    Note = e.Note,
                    SectionImageId = e.Id,
                    Path = e.Document.Path,
                });

                return operation.SetSuccess(dto);
            };
        private Func<OperationResult<QuestionTagsAnswersExamsDto>, Task<OperationResult<QuestionTagsAnswersExamsDto>>> _details(Guid id)
            => async operation => {
                var one = await Query.Where(question => question.Id == id
                                                     && !question.DateDeleted.HasValue)
                                     .Select(question => new QuestionTagsAnswersExamsDto()
                                     {
                                         Id = question.Id,
                                         AnswerType = question.AnswerType,
                                         Hint = question.Hint,
                                         Title = question.Title,
                                         DateCreated = question.DateCreated,
                                         Tags = question.QuestionTags.Select(t => t.TagId),
                                         QuestionType = question.QuestionType,
                                         IsCorrected = question.IsCorrected,
                                         Exams = question.ExamQuestions.Select(e => new QuestionExamDto()
                                         {
                                             Order = e.Order,
                                             ExamId = e.ExamId,
                                             QuestionId = e.QuestionId,
                                             DateCreated = e.DateCreated,
                                             ExamName = e.Exam.Name
                                         }),
                                         Images = question.QuestionDocuments.Select(e => new QuestionDocumentNote
                                         {
                                             SectionImageId = e.Id,
                                             Note = e.Note,
                                             Path = e.Document.Path,
                                         }),
                                         Answers = question.Answers.Select(answer => new AnswerDto()
                                         {
                                             Id = answer.Id,
                                             CorrectAnswerId = answer.CorrectAnswerId,
                                             CorrectionDate = answer.CorrectionDate,
                                             QuestionId = answer.QuestionId,
                                             Title = answer.Title,
                                             IsCorrect = answer.IsCorrect,
                                         })
                                     }).FirstOrDefaultAsync();

                return operation.SetSuccess(one);
            };
        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _delete(Guid id)
            => async operation =>
            {
                await Context.SoftDeleteTraversalAsync((Expression<Func<Question, bool>>)(question => question.Id == id),
                    question => question.Answers, "ExamQuestions");
                await Context.SaveChangesDeletedAsync();
                var toRemove = await _query<QuestionDocument>().Include(x => x.Document)
                                                               .Where(x => x.QuestionId == id).ToListAsync();
                Context.RemoveRange(toRemove);
                var removePaths = toRemove.Select(x => x.Document?.Path);
                foreach (var path in removePaths)
                    TryDeleteImage(path);
                await Context.SaveChangesAsync();
                return operation.SetSuccess(true, $"Soft deleted traversal(Answers,ExamQuestions) , Remove QuestionDocuments , Documnets  {typeof(Question).Name} success");
            };
        private Func<OperationResult<QuestionAnswersDto>, Task<OperationResult<QuestionAnswersDto>>> _correctQuestion(QuestionAnswersDto question)
            => async operation => {
                var ques = await TrackingQuery.Include(q => q.Answers)
                                              .Where(q => q.Id == question.Id)
                                              .FirstOrDefaultAsync();
                if (ques.Title != question.Title)
                {
                    ques.Title = question.Title;
                    ques.IsCorrected = true;
                }
                foreach (var answer in question.Answers)
                {
                    if (answer.Id == Guid.Empty)
                    {
                        if (answer.CorrectAnswerId.HasValue && answer.CorrectAnswerId != Guid.Empty)
                        {
                            var correctAnswer = ques.Answers.FirstOrDefault(a => a.Id == answer.CorrectAnswerId);
                            if (correctAnswer is not null)
                            {
                                correctAnswer.CorrectionDate = DateTime.Now.ToLocalTime();
                                ques.Answers.Add(new Answer
                                {
                                    CorrectAnswerId = correctAnswer.Id, //answer.CorrectAnswerId
                                    IsCorrect = answer.IsCorrect,
                                    Title = answer.Title,
                                });
                                ques.IsCorrected = true;
                            }
                            else
                            {
                                return operation.SetFailed($"CorrectAnswerId :{answer.CorrectAnswerId} , un-correct with related answer {answer.Title}");
                            }
                            // error from clinet
                        }
                        else
                        {
                            // error from clinet
                            return operation.SetFailed($"CorrectAnswerId un-assign with Empty Guid {answer.Title}");
                        }
                    }
                }
                await Context.SaveChangesAsync();
                return operation.SetSuccess(new QuestionAnswersDto
                {
                    Id = ques.Id,
                    AnswerType = ques.AnswerType,
                    DateCreated = ques.DateCreated,
                    IsCorrected = ques.IsCorrected,
                    Hint = ques.Hint,
                    QuestionType = ques.QuestionType,
                    Title = ques.Title,
                    Answers = ques.Answers.Select(ans => new AnswerDto
                    {
                        Id = ans.Id,
                        Title = ans.Title,
                        CorrectAnswerId = ans.CorrectAnswerId,
                        CorrectionDate = ans.CorrectionDate,
                        IsCorrect = ans.IsCorrect,
                        QuestionId = ans.QuestionId
                    })
                });
            };



        /// TODO waiting Elkood File, ElFileUpload
        private async Task<OperationResult<bool>> TryUploadImageAsync(IFormFile image)
        {
            string path = null;
            try
            {
                if (image != null)
                {
                    var documentsDirectory = Path.Combine("wwwroot", "Documents", "Exam_Image");

                    if (!Directory.Exists(documentsDirectory))
                        Directory.CreateDirectory(documentsDirectory);

                    path = Path.Combine("Documents", "Exam_Image", Guid.NewGuid().ToString() + "_" + image.FileName);
                    string filePath = Path.Combine(webHostEnvironment.WebRootPath, path);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                }
                return new OperationResult<bool>().SetSuccess(true, path);
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
    }
}
