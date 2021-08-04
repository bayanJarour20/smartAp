using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.BoundedContext.General;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.SubjectDto;
using SmartStart.Model.Main;
using SmartStart.Model.Shared;
using SmartStart.SharedKernel.Enums;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Main.SubjectService
{
    [ElRepository]
    public class SubjectRepository : ElRepository<SmartStartDbContext, Guid, Subject>, ISubjectRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public SubjectRepository(SmartStartDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<OperationResult<IEnumerable<SubjectDto>>> GetAll(int? year, Guid? semesterId, Guid? facultyId)
            => await RepositoryHandler(_getAll(year, semesterId, facultyId));
        public async Task<OperationResult<SubjectDto>> SetSubject(SubjectDetailsDto subjectDto)
            => await RepositoryHandler(_setSubject(subjectDto));
        public async Task<OperationResult<SubjectAllDto>> SubjectDetails(Guid subjectId)
            => await RepositoryHandler(_subjectDetails(subjectId));
        public async Task<OperationResult<bool>> RemoveSubject(Guid subjectId)
            => await RepositoryHandler(_removeSubject(subjectId));


        private Func<OperationResult<IEnumerable<SubjectDto>>, Task<OperationResult<IEnumerable<SubjectDto>>>> _getAll(int? year, Guid? semesterId, Guid? facultyId)
            => async operation => {

                var res = await Query.Where(s => s.Faculties.Any(f => f.Year == year) 
                                            && s.Faculties.Any(f => f.SemesterId == semesterId)
                                            && s.Faculties.Any(f => f.FacultyId == facultyId))
                                    .Select(s => new SubjectDto 
                                    {
                                        Id = s.Id, 
                                        Name = s.Name,
                                        BankCount = s.Exams.Count(e => e.Type == TabTypes.Bank),
                                        ExamCount = s.Exams.Count(e => e.Type == TabTypes.Exam),
                                        InterviewCount = s.Exams.Count(e => e.Type == TabTypes.Interview),
                                        MicroscopeCount = s.Exams.Count(e => e.Type == TabTypes.Microscope),
                                        DateCreate = s.DateCreated
                                    }).ToListAsync();
                return operation.SetSuccess(res);
            };
        private Func<OperationResult<SubjectDto>, Task<OperationResult<SubjectDto>>> _setSubject(SubjectDetailsDto subjectDto)
            => async operation => {
                Subject subject; 
                if(subjectDto.Id == Guid.Empty)
                {
                    subject = new Subject
                    {
                        Id = new Guid(),
                        Description = subjectDto.Description,
                        ImagePath = subjectDto.ImagePath,
                        IsFree = subjectDto.IsFree,
                        Name = subjectDto.Name,
                        Type = subjectDto.Type,
                    };
                    await Context.Subjects.AddAsync(subject);
                }
                else 
                {
                    subject = await TrackingQuery.Include(s => s.SubjectTags)
                                                 .ThenInclude(t => t.Tag)
                                                 .Where(s => s.Id == subjectDto.Id).SingleOrDefaultAsync();
                    subject.Description = subjectDto.Description;
                    subject.ImagePath = subjectDto.ImagePath;
                    subject.IsFree = subjectDto.IsFree;
                    subject.Name = subjectDto.Name;
                    subject.Type = subjectDto.Type;
                    Context.Subjects.Update(subject);
                    var subjectDoctors = subject.SubjectTags.Where(t => t.Tag.Type == TagTypes.Doctor);
                    Context.RemoveRange(subjectDoctors);
                }
                if(subjectDto.Doctors != null)
                {
                    List<SubjectTag> temp = new List<SubjectTag>(); 
                    foreach (var doctor in subjectDto.Doctors)
                    {
                        temp.Add(new SubjectTag
                        {
                            SubjectId = subject.Id,
                            TagId = doctor
                        });
                    }
                    await Context.SubjectTags.AddRangeAsync(temp);
                }
                await Context.SaveChangesAsync(); 
                return operation.SetSuccess(new SubjectDto
                {
                    Id = subject.Id,
                    Name = subject.Name,
                    BankCount = subject.Exams.Count(e => e.Type == TabTypes.Bank), 
                    ExamCount = subject.Exams.Count(e => e.Type == TabTypes.Exam), 
                    InterviewCount = subject.Exams.Count(e => e.Type == TabTypes.Interview),
                    MicroscopeCount = subject.Exams.Count(e => e.Type == TabTypes.Microscope),
                    DateCreate = subject.DateCreated,
                }); 
            };
        private Func<OperationResult<SubjectAllDto>, Task<OperationResult<SubjectAllDto>>> _subjectDetails(Guid subjectId)
            => async operation => {

                var res = await Query.Include(s => s.SubjectTags)
                                     .ThenInclude(t => t.Tag)
                                     .Where(s => s.Id == subjectId)
                                     .Select(s => new SubjectAllDto
                                     {
                                         Id = s.Id,
                                         Name = s.Name,
                                         Description = s.Description,
                                         ImagePath = s.ImagePath,
                                         IsFree = s.IsFree,
                                         Type = s.Type,
                                         Doctors = s.SubjectTags.Where(t => t.Tag.Type == TagTypes.Doctor)
                                                                .Select(t => t.TagId).ToList(),
                                         BankCount = s.Exams.Count(e => e.Type == TabTypes.Bank),
                                         ExamCount = s.Exams.Count(e => e.Type == TabTypes.Exam),
                                         InterviewCount = s.Exams.Count(e => e.Type == TabTypes.Interview),
                                         MicroscopeCount = s.Exams.Count(e => e.Type == TabTypes.Microscope),
                                         DateCreate = s.DateCreated
                                     }).SingleOrDefaultAsync();
                return operation.SetSuccess(res);
            };
        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _removeSubject(Guid subjectId)
            => async operation => {
                var subject = await TrackingQuery.Include(s => s.Exams)
                                                 .Include(s => s.SubjectTags)
                                                 .Where(s => s.Id == subjectId).SingleOrDefaultAsync();
                if(subject == null)
                {
                    return operation.SetSuccess(false);
                }
                subject.DateDeleted = DateTime.Now;
                foreach (var exam in subject.Exams)
                {
                    exam.DateDeleted = DateTime.Now;
                }
                foreach (var tag in subject.SubjectTags)
                {
                    tag.DateDeleted = DateTime.Now;
                }
                Context.Update(subject);
                await Context.SaveChangesAsync();
                return operation.SetSuccess(true);
            };





    }
}
