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

        
        private Func<OperationResult<IEnumerable<ExamDetailsDto>>, Task<OperationResult<IEnumerable<ExamDetailsDto>>>> _getAllExam()
            => async operation =>
            {
                var Exams = await Query.Where(exam => exam.Type == TabTypes.Exam)
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
                return operation.SetSuccess(Exams);
            };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _deleteExam(Guid id)
            => async operation =>
            {
                var exam = await FindAsync(id);
                if (exam == null)
                {
                    return (OperationResultTypes.NotExist, $"{id} : not exsit.");
                }
                var Exam = TrackingQuery.Where(e => e.Id == exam.Id).Include()
                

            };

    }
}
