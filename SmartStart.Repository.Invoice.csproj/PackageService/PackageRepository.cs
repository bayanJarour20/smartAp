using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.PackageDto;
using SmartStart.Model.Business;
using SmartStart.Model.Main;
using SmartStart.SharedKernel.Enums;
using SmartStart.SharedKernel.ExtensionMethods;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Invoice.PackageService
{
    [ElRepository]
    public class PackageRepository : ElRepository<SmartStartDbContext, Guid, Package>, IPackageRepository
    {
        public PackageRepository(SmartStartDbContext context) : base(context) { }

        public async Task<OperationResult<IEnumerable<PackageDetailsDto>>> GetAll()
        => await RepositoryHandler(_getAll());

        public async Task<OperationResult<PackageSubjectFilterDto>> Details(Guid id)
         => await RepositoryHandler(_details(id));


        public async Task<OperationResult<PackageDto>> Add(PackageSubjectDto dto)
        => await RepositoryHandler(_add(dto));

        public async Task<OperationResult<PackageDto>> Update(PackageSubjectDto dto)
        => await RepositoryHandler(_update(dto));

        public async Task<OperationResult<bool>> Delete(Guid Id)
        => await RepositoryHandler(_delete(Id));

        //public async Task<OperationResult<bool>> Init()
        // => await RepositoryHandler<bool>(async operation => {

        //     string[] yearNames = { "السنة الأولى", "السنة الثانية", "السنة الثالثة", "السنة الرابعة", "السنة الخامسة", "السنة السادسة", "السنة السابعة", "السنة الثامنة" };

        //     var subjects = await Context.Subjects
        //                                 .Include(s => s.Packages)
        //                                 .ThenInclude(p => p.Package)
        //                                 .ToListAsync();

        //     var groupYear = subjects.GroupBy(x => (x.Year, x.FacultyId));


        //     foreach (var group in groupYear)
        //     {
        //         int fixYear = group.Key.Year == 0 || group.Key.Year > yearNames.Length ? 1 : group.Key.Year - 1;

        //         string packageName = group.First().Faculty.Name + " " + yearNames[fixYear];

        //         var exams = group.SelectMany(g => g.Exams);

        //         var backpackage = await Context.Packages.Include(p => p.PackageSubjects).FirstOrDefaultAsync(p => p.Name == packageName);

        //         if (backpackage is null)
        //         {
        //             Package package = new()
        //             {
        //                 Name = packageName,
        //                 Price = 4000,
        //                 Type = PackageTypes.Normal,
        //                 Description = $"تحوي على كافة الدورات المواد التابعة لـ{packageName}",
        //                 PackageSubjects = exams.Select(e => new PackageSubject()
        //                 {
        //                     SubjectId = e.Id,
        //                 }).ToList()
        //             };
        //             await Context.Packages.AddAsync(package);
        //             await Context.SaveChangesAsync();
        //         }
        //         else
        //         {
        //             var except = ExtensionMethodsShared.IsolatedExcept(backpackage.PackageSubjects, exams, e => e.SubjectId, e => e.Id);
        //             Context.PackageSubjects.RemoveRange(Context.PackageSubjects.Where(x => x.PackageId == backpackage.Id && except.remove.Contains(x.SubjectId)));
        //             await Context.PackageSubjects.AddRangeAsync(except.Add.Select(x => new PackageSubject()
        //             {

        //                 PackageId = backpackage.Id,
        //                 SubjectId = x,
        //             }));
        //             await Context.SaveChangesAsync();
        //         }
        //     }
        //     return operation.SetSuccess(true);
        // });



        private Func<OperationResult<IEnumerable<PackageDetailsDto>>, Task<OperationResult<IEnumerable<PackageDetailsDto>>>> _getAll()
        => async operation => {

            var list = await Query.Select(package => new PackageDetailsDto
            {
                Id = package.Id,
                Name = package.Name,
                Description = package.Description,
                EndDate = package.EndDate,
                CreateDate = package.DateCreated,
                Price = package.Price,
                StartDate = package.StartDate,
                Type = package.Type,
                IsHidden = package.IsHidden,
                CodeCount = package.CodePackages.Count()
            }).ToListAsync();

            return operation.SetSuccess(list.OrderByDescending(w => w.CreateDate).ToList());
        };


        private Func<OperationResult<PackageSubjectFilterDto>, Task<OperationResult<PackageSubjectFilterDto>>> _details(Guid id)
        => async operation => {

            var one = await Query.Where(package => package.Id == id).Select(package => new PackageSubjectFilterDto
            {
                Id = package.Id,
                Name = package.Name,
                Description = package.Description,
                Price = package.Price,
                StartDate = package.StartDate,
                EndDate = package.EndDate,
                Type = package.Type,
                IsHidden = package.IsHidden,
                Subjects = package.PackageSubjects.Select(pe => new PriceSubjectDto
                {
                    Price = pe.Price,
                    SubjectId = pe.SubjectId,
                }),
            }).FirstOrDefaultAsync();

            object filter = new
            {
                FacultyId = Guid.Empty,
                Year = 1,
                SemesterId = Guid.Empty,
            };

            //var anySubject = one.Subjects.FirstOrDefault();
            //if (anySubject is not null)
            //{
            //    filter = await _query<Subject>().Where(subject => subject.Id == anySubject.SubjectId).Select(subject =>
            //        new
            //        {
            //            FacultyId = subject.Faculties.Select(),
            //            Year = subject.Faculties.S,
            //            SemesteId = subject.SubjectTags.Where(s => s.Tag.Type == TagTypes.Semester).Select(x => x.TagId).FirstOrDefault(),
            //        }).FirstOrDefaultAsync();
            //}
            one.Filter = filter;

            return operation.SetSuccess(one);
        };


        private Func<OperationResult<PackageDto>, Task<OperationResult<PackageDto>>> _add(PackageSubjectDto dto)
        => async operation =>
        {
            if (dto.Subjects is null || !dto.Subjects.Any())
                return operation.SetFailed($"It must contain a {nameof(dto.Subjects)}");

            Package one = new()
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                EndDate = dto.EndDate,
                Price = dto.Price,
                StartDate = dto.StartDate,
                Type = dto.Type,
                IsHidden = dto.IsHidden,
                PackageSubjects = dto.Subjects.Select(e => new PackageSubject() { Price = e.Price, SubjectId = e.SubjectId }).ToList(),
            };

            await Context.AddAsync(one);
            await Context.SaveChangesAsync();
            dto.Id = one.Id;

            return operation.SetSuccess(new PackageDto()
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                EndDate = dto.EndDate,
                Price = dto.Price,
                StartDate = dto.StartDate,
                Type = dto.Type,
                IsHidden = dto.IsHidden,
            });
        };


        private Func<OperationResult<PackageDto>, Task<OperationResult<PackageDto>>> _update(PackageSubjectDto dto)
         => async operation =>
         {
             var one = await FindAsync(dto.Id);

             if (dto.Subjects is null || !dto.Subjects.Any())
                 return operation.SetFailed($"It must contain a {nameof(dto.Subjects)}");

             Context.RemoveRange(await Context.PackageSubjects.Where(c => c.PackageId == one.Id).ToListAsync());

             one.Name = dto.Name;
             one.Description = dto.Description;
             one.EndDate = dto.EndDate;
             one.Price = dto.Price;
             one.StartDate = dto.StartDate;
             one.Type = dto.Type;
             one.IsHidden = dto.IsHidden;
             one.PackageSubjects = dto.Subjects.Select(e => new PackageSubject() { Price = e.Price, SubjectId = e.SubjectId }).ToList();

             await Context.SaveChangesAsync();

             return operation.SetSuccess(new PackageDto()
             {
                 Id = dto.Id,
                 Name = dto.Name,
                 Description = dto.Description,
                 EndDate = dto.EndDate,
                 Price = dto.Price,
                 StartDate = dto.StartDate,
                 Type = dto.Type,
                 IsHidden = dto.IsHidden,
             });
         };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _delete(Guid id)
        => async operation =>
        {
            var one = await FindAsync(id);
            if (one is null)
                return (OperationResultTypes.NotExist, $"{id} not exist");

            await Context.SoftDeleteTraversalAsync((Expression<Func<Package, bool>>)(p => p.Id == id), p => p.PackageSubjects);
            await Context.SaveChangesDeletedAsync();

            return operation.SetSuccess(true, "Success delete");
        };


    }
}
