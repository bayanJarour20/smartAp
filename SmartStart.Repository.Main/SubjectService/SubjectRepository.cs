﻿using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Helper.ExtensionMethods.String;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.BoundedContext.General;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.SubjectDto;
using SmartStart.Model.Main;
using SmartStart.Model.Shared;
using SmartStart.SharedKernel.Enums;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.IO;
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
        public async Task<OperationResult<SubjectAllDto>> SetSubject(SubjectDetailsDto subjectDto, IFormFile image)
            => await RepositoryHandler(_setSubject(subjectDto, image));
        public async Task<OperationResult<SubjectAllDto>> SubjectDetails(Guid subjectId)
            => await RepositoryHandler(_subjectDetails(subjectId));
        public async Task<OperationResult<bool>> RemoveSubject(Guid subjectId)
            => await RepositoryHandler(_removeSubject(subjectId));
        public async Task<OperationResult<bool>> RemoveSubjects(List<Guid> subjectIds)
            => await RepositoryHandler(_removeSubjects(subjectIds));


        private Func<OperationResult<IEnumerable<SubjectDto>>, Task<OperationResult<IEnumerable<SubjectDto>>>> _getAll(int? year, Guid? semesterId, Guid? facultyId)
            => async operation => {

                var res = await Query.Where(s => (year == null || s.SubjectFaculties.Any(f => f.Year == year))
                                              && (semesterId == null || s.SubjectFaculties.Any(f => f.SemesterId == semesterId))
                                              && (facultyId == null || s.SubjectFaculties.Any(f => f.FacultyId == facultyId)))
                                     .Select(s => new SubjectDto
                                     {
                                         Id = s.Id,
                                         Name = s.Name,
                                         BankCount = s.Exams.Count(e => e.Type == TabTypes.Bank),
                                         ExamCount = s.Exams.Count(e => e.Type == TabTypes.Exam),
                                         InterviewCount = s.Exams.Count(e => e.Type == TabTypes.Interview),
                                         MicroscopeCount = s.Exams.Count(e => e.Type == TabTypes.Microscope),
                                         DateCreate = s.DateCreated,
                                         subjectFaculties = s.SubjectFaculties.Select(f => new SubjectFacultyDto 
                                         {
                                             FacultyId = f.FacultyId,
                                             SectionId = f.SectionId, 
                                             SemesterId = f.SemesterId,
                                             DoctorId = f.DoctorId,
                                             Year = f.Year,
                                             Price = f.Price
                                         }).ToList()
                                         
                                     }).ToListAsync();
                return operation.SetSuccess(res);
            };
        private Func<OperationResult<SubjectAllDto>, Task<OperationResult<SubjectAllDto>>> _setSubject(SubjectDetailsDto subjectDto, IFormFile image)
            => async operation => {
                Subject subject; 
                if(subjectDto.Id == Guid.Empty)
                {
                    var result = TryUploadImage(image, out string path);
                    if (result.IsSuccess)
                    {
                        subjectDto.ImagePath = path; 
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
                        subjectDto.Id = subject.Id;
                        //await addSubjectDoctors(subjectDto.Doctors, subject.Id);
                        await addSubjectFaculties(subjectDto.subjectFaculties, subject.Id);
                        await Context.SaveChangesAsync();
                        return operation.SetSuccess(new SubjectAllDto
                        {
                            Id = subjectDto.Id,
                            DateCreate = subject.DateCreated,
                            BankCount = 0,
                            ExamCount = 0,
                            InterviewCount = 0, 
                            MicroscopeCount = 0, 
                            ImagePath = subject.ImagePath,
                            IsFree = subject.IsFree,
                            Name = subject.Name,
                            Type = subject.Type,
                            Description = subject.Description,
                            subjectFaculties = subjectDto.subjectFaculties
                        });
                    }
                    return operation.SetFailed("Failed upload, Message: " + result.FullExceptionMessage);
                }
                subject = await TrackingQuery.Include(s => s.SubjectTags)
                                             .ThenInclude(t => t.Tag)
                                             .Include(s => s.SubjectFaculties)
                                             .Where(s => s.Id == subjectDto.Id).SingleOrDefaultAsync();
                if (subject is not null)
                {
                    if (subject.ImagePath != subjectDto.ImagePath || image != null)
                    {
                        TryDeleteImage(subject.ImagePath);
                    }
                    var result = TryUploadImage(image, out string path);
                    if (result.IsSuccess)
                    {
                        subject.ImagePath = (!subjectDto.ImagePath.IsNullOrEmpty() && image is null) ? subject.ImagePath : path;
                        subject.Description = subjectDto.Description;
                        subject.ImagePath = subjectDto.ImagePath;
                        subject.IsFree = subjectDto.IsFree;
                        subject.Name = subjectDto.Name;
                        subject.Type = subjectDto.Type;
                        Context.Subjects.Update(subject);
                        await Context.SaveChangesAsync();
                    }
                    else
                    {
                        return operation.SetFailed("Failed upload, Message: " + result.FullExceptionMessage);
                    }
                }
                else 
                {
                    return (OperationResultTypes.NotExist, $"Id :{subjectDto.Id}");
                }

                var subjectDoctors = subject.SubjectTags.Where(t => t.Tag.Type == TagTypes.Doctor).ToList();
                
                Context.SubjectTags.RemoveRange(subjectDoctors);

                //await addSubjectDoctors(subjectDto.Doctors, subject.Id);

                var subjectFaculties = subject.SubjectFaculties.ToList();
                
                Context.SubjectFaculties.RemoveRange(subjectFaculties);
                
                await addSubjectFaculties(subjectDto.subjectFaculties, subject.Id);

                await Context.SaveChangesAsync(); 
                
                return operation.SetSuccess(new SubjectAllDto
                {
                    Id = subject.Id,
                    BankCount = subject.Exams.Count(e => e.Type == TabTypes.Bank),
                    ExamCount = subject.Exams.Count(e => e.Type == TabTypes.Exam),
                    InterviewCount = subject.Exams.Count(e => e.Type == TabTypes.Interview),
                    MicroscopeCount = subject.Exams.Count(e => e.Type == TabTypes.Microscope),
                    DateCreate = subject.DateCreated,
                    Description = subject.Description,
                    IsFree = subject.IsFree,
                    ImagePath = subject.ImagePath,
                    Name = subject.Name,
                    Type = subject.Type,
                    subjectFaculties = subjectDto.subjectFaculties
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
                                         //Doctors = s.SubjectTags.Where(t => t.Tag.Type == TagTypes.Doctor)
                                         //                       .Select(t => t.TagId).ToList(),
                                         BankCount = s.Exams.Count(e => e.Type == TabTypes.Bank),
                                         ExamCount = s.Exams.Count(e => e.Type == TabTypes.Exam),
                                         InterviewCount = s.Exams.Count(e => e.Type == TabTypes.Interview),
                                         MicroscopeCount = s.Exams.Count(e => e.Type == TabTypes.Microscope),
                                         DateCreate = s.DateCreated,
                                         subjectFaculties = s.SubjectFaculties.Select(f => new SubjectFacultyDto
                                         {
                                             FacultyId = f.FacultyId,
                                             FacultyName = f.Faculty != null? f.Faculty.Name : "",
                                             SectionId = f.SectionId,
                                             SectionName = f.Section != null? f.Section.Name : "",
                                             SemesterId = f.SemesterId,
                                             SemesterName = f.Semester != null? f.Semester.Name : "",
                                             DoctorId = f.DoctorId,
                                             DoctorName = f.Doctor != null? f.Doctor.Name : "",
                                             Year = f.Year,
                                             Price = f.Price
                                         }).ToList()
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
        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _removeSubjects(List<Guid> subjectIds)
            => async operation => {
                var subjects = await TrackingQuery.Where(s => subjectIds.Contains(s.Id)).ToListAsync();
                if (subjects == null)
                {
                    return operation.SetSuccess(false);
                }
                foreach (var faculty in subjects)
                {
                    faculty.DateDeleted = DateTime.Now;
                }
                Context.UpdateRange(subjects);
                await Context.SaveChangesAsync();
                return operation.SetSuccess(true);
            };


        #region Helper Functions
        private OperationResult<bool> TryUploadImage(IFormFile image, out string path)
        {
            path = null;
            try
            {
                if (image != null)
                {
                    var documentsDirectory = Path.Combine("wwwroot", "Documents", "Subject_Image");
                    if (!Directory.Exists(documentsDirectory))
                    {
                        Directory.CreateDirectory(documentsDirectory);
                    }
                    path = Path.Combine("Documents", "Subject_Image", Guid.NewGuid().ToString() + "_" + image.FileName);
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
        private async Task addSubjectDoctors(List<Guid> doctors, Guid subjectId)
        {
            if (doctors != null)
            {
                List<SubjectTag> temp = new List<SubjectTag>();
                foreach (var doctor in doctors)
                {
                    temp.Add(new SubjectTag
                    {
                        SubjectId = subjectId,
                        TagId = doctor
                    });
                }
                await Context.SubjectTags.AddRangeAsync(temp);
            }
        }
        private async Task addSubjectFaculties(List<SubjectFacultyDto> faculties, Guid subjectId)
        {
            if (faculties != null)
            {
                List<SubjectFaculty> temp = new List<SubjectFaculty>();
                foreach (var faculty in faculties)
                {
                    temp.Add(new SubjectFaculty
                    {
                        FacultyId = faculty.FacultyId,
                        SectionId = faculty.SectionId,
                        SemesterId = faculty.SemesterId,
                        DoctorId =faculty.DoctorId,
                        Year = faculty.Year,
                        SubjectId = subjectId,
                        Price = faculty.Price
                    });
                }
                await Context.SubjectFaculties.AddRangeAsync(temp);
            }
        }
        #endregion
    }
}
