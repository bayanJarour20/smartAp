using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Helper.ExtensionMethods.String;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.FacultyDto;
using SmartStart.DataTransferObject.SharedDto;
using SmartStart.DataTransferObject.SubjectDto;
using SmartStart.Model.General;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.General.FacultyService
{
    [ElRepository]
    public class FacultyRepository : ElRepository<SmartStartDbContext, Guid, Faculty>, IFacultyRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public FacultyRepository(SmartStartDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<OperationResult<IEnumerable<FacultyBaseDto>>> GetAll()
            => await RepositoryHandler(_getAll());
        public async Task<OperationResult<FacultyDto>> SetFaculty(FacultyDto facultyDto, IFormFile formFile)
            => await RepositoryHandler(_setFaculty(facultyDto, formFile));
        public async Task<OperationResult<bool>> RemoveFaculty(Guid facultyId)
            => await RepositoryHandler(_removeFaculty(facultyId));
        public async Task<OperationResult<bool>> RemoveFaculties(List<Guid> facultyIds)
            => await RepositoryHandler(_removeFaculties(facultyIds));
        public async Task<OperationResult<IEnumerable<SelectDto>>> FacultySelect()
            => await RepositoryHandler(_facultySelect());



        private Func<OperationResult<IEnumerable<FacultyBaseDto>>, Task<OperationResult<IEnumerable<FacultyBaseDto>>>> _getAll()
            => async operation => {

                var res = await Query.Select(s => new FacultyBaseDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    NumOfYears = s.NumberOfYear,
                    SubjectCount = s.Subjects.Count()
                }).ToListAsync();
                return operation.SetSuccess(res);
            };
        private Func<OperationResult<FacultyDto>, Task<OperationResult<FacultyDto>>> _setFaculty(FacultyDto facultyDto, IFormFile image)
            => async operation => {
                Faculty faculty; 
                if (facultyDto.Id == Guid.Empty)
                {
                    var result = TryUploadImage(image, out string path);
                    if (result.IsSuccess)
                    {
                        facultyDto.ImagePath = path;
                        faculty = new Faculty
                        {
                            Id = new Guid(), 
                            ImagePath = path, 
                            Name = facultyDto.Name,
                            NumberOfYear = facultyDto.NumOfYears,
                            UniversityId = facultyDto.UniversityId
                        };
                        await Context.Faculties.AddAsync(faculty);
                        facultyDto.Id = faculty.Id;
                        await Context.SaveChangesAsync(); 
                        return operation.SetSuccess(facultyDto);
                    }
                    return operation.SetFailed("Failed upload, Message: " + result.FullExceptionMessage);
                }
                faculty = await TrackingQuery.Where(f => f.Id == facultyDto.Id).SingleOrDefaultAsync(); 
                if (faculty is not null)
                {
                    if (faculty.ImagePath != facultyDto.ImagePath || image != null)
                    {
                        TryDeleteImage(faculty.ImagePath);
                    }
                    var result = TryUploadImage(image, out string path);
                    if (result.IsSuccess)
                    {
                        faculty.ImagePath = (!facultyDto.ImagePath.IsNullOrEmpty() && image is null) ? faculty.ImagePath : path;
                        faculty.Name = facultyDto.Name;
                        faculty.NumberOfYear = facultyDto.NumOfYears;
                        faculty.UniversityId = facultyDto.UniversityId;
                        Context.Faculties.Update(faculty);
                        await Context.SaveChangesAsync(); 
                        return operation.SetSuccess(facultyDto);
                    }
                    return operation.SetFailed("Failed upload, Message: " + result.FullExceptionMessage);
                }
                return (OperationResultTypes.NotExist, $"Id :{facultyDto.Id}");
            };
        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _removeFaculty(Guid facultyId)
           => async operation => {
               var faculty = await TrackingQuery.Where(s => s.Id == facultyId).SingleOrDefaultAsync();
               if (faculty == null)
               {
                   return operation.SetSuccess(false);
               }
               faculty.DateDeleted = DateTime.Now;
               Context.Update(faculty);
               await Context.SaveChangesAsync();
               return operation.SetSuccess(true);
           };
        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _removeFaculties(List<Guid> facultyIds)
            => async operation => {
                var faculties = await TrackingQuery.Where(s => facultyIds.Contains(s.Id)).ToListAsync();
                if (faculties == null)
                {
                    return operation.SetSuccess(false);
                }
                foreach (var faculty in faculties)
                {
                    faculty.DateDeleted = DateTime.Now;
                }
                Context.UpdateRange(faculties);
                await Context.SaveChangesAsync();
                return operation.SetSuccess(true);
            };
        private Func<OperationResult<IEnumerable<SelectDto>>, Task<OperationResult<IEnumerable<SelectDto>>>> _facultySelect()
            => async operation => {
                var res = await Query.Select(s => new SelectDto
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToListAsync();
                return operation.SetSuccess(res);
            };




        #region Helper Functions
        private OperationResult<bool> TryUploadImage(IFormFile image, out string path)
        {
            path = null;
            try
            {
                if (image != null)
                {
                    var documentsDirectory = Path.Combine("wwwroot", "Documents", "Faculty_Image");
                    if (!Directory.Exists(documentsDirectory))
                    {
                        Directory.CreateDirectory(documentsDirectory);
                    }
                    path = Path.Combine("Documents", "Faculty_Image", Guid.NewGuid().ToString() + "_" + image.FileName);
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
