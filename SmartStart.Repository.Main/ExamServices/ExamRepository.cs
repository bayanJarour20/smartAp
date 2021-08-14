using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.ExamDto;
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
                                  Price = exam.Price,
                                  IsFree = exam.IsFree,
                                  QuestionsCount = exam.ExamQuestions.Count(),
                                  SemesterId = exam.Subject.SubjectTags.Where(s => s.Tag.Type == TagTypes.Semester).Select(s => s.Id).FirstOrDefault(),
                                  TagIds = exam.ExamTags.Select(et => et.Id),
                              }).ToListAsync();
        }

        private async Task<bool> TryDelete(Guid examId, TabTypes examType)
        {
            var one = await TrackingQuery.Where(exam => exam.Id == examId).Include(e => e.ExamQuestions).Include(e => e.ExamTags).FirstOrDefaultAsync();
            if (one is null)
                return false;
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

        #endregion

    }
}
