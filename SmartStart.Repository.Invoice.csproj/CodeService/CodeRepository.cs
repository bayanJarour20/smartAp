using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.CodeDto;
using SmartStart.Model.Business;
using SmartStart.Model.Main;
using SmartStart.SharedKernel.Enums;
using SmartStart.SharedKernel.ExtensionMethods;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Invoice.CodeService
{
    [ElRepository]
    public class CodeRepository : ElRepository<SmartStartDbContext, Guid, Code>, ICodeRepository
    {
        public CodeRepository(SmartStartDbContext context) : base(context) { }


        public async Task<OperationResult<IEnumerable<CodeSubjectsPriceDto>>> GetCodes(Guid userId)
            => await RepositoryHandler(_getCodes(userId));

        public async Task<OperationResult<bool>> ActivateCode(Guid userId, string hash)
            => await RepositoryHandler(_activateCode(userId, hash));

        //public async Task<OperationResult<bool>> ActivateCodeV2(string hash, IEnumerable<Guid> subjectIds)
        //    => await RepositoryHandler(_activateCodeV2(hash, subjectIds));

        //public async Task<OperationResult<object>> CheckCode(string hash)
        //    => await RepositoryHandler(_checkCode(hash));

        //public async Task<OperationResult<IEnumerable<CodeDetailsDto>>> GetAll()
        //    => await RepositoryHandler(_getAll());

        //public async Task<OperationResult<CodeDetailsDto>> Generate(CodeGenerateDto dto)
        //    => await RepositoryHandler(_generate(dto));
        
        //public async Task<OperationResult<CodeDetailsDto>> Update(CodeGenerateDto dto)
        //    => await RepositoryHandler(_update(dto));

        //public async Task<OperationResult<bool>> Delete(Guid id)
        //=> await RepositoryHandler(_delete(id));


        private Func<OperationResult<IEnumerable<CodeSubjectsPriceDto>>, Task<OperationResult<IEnumerable<CodeSubjectsPriceDto>>>> _getCodes(Guid userId)
            => async operation => {
                var list = (await Query.Include(code => code.CodePackages)
                                       .ThenInclude(p => p.Package)
                                       .ThenInclude(p => p.PackageSubjectFaculties)
                                       .Where(code => code.UserId == userId)
                                       .Select(code => new
                                       {
                                           Id = code.Id,
                                           Value = code.Value,
                                           Hash = code.Hash,
                                           MaxEndDate = code.MaxEndDate,
                                           Subjects = code.CodePackages.SelectMany(cp => cp.Package.PackageSubjectFaculties
                                                                       .Select(pe => new
                                                                       {
                                                                           pe.Price,
                                                                           pe.SubjectFaculty.SubjectId,
                                                                           pe.SubjectFaculty.Subject.Name
                                                                       })),
                                       }).ToListAsync())
                                       .Select(code => new CodeSubjectsPriceDto
                                       {
                                           Id = code.Id,
                                           Value = code.Value,
                                           Hash = code.Hash,
                                           MaxEndDate = code.MaxEndDate,
                                           Subjects = code.Subjects.GroupBy(subject => subject.SubjectId, pe => new
                                           {
                                               pe.SubjectId,
                                               pe.Price,
                                               pe.Name
                                           }).Select(g => new BaseSubjectPriceDto
                                           {
                                               SubjectId = g.Key,
                                               Price = g.Sum(subject => subject.Price),
                                               SubjectName = g.First().Name,
                                           })
                                       });
                return operation.SetSuccess(list);
            };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _activateCode(Guid userId, string hash)
            => async operation => {
                var one = await TrackingQuery.Include(x => x.CodePackages)
                                             .ThenInclude(x => x.Package)
                                             .ThenInclude(x => x.PackageSubjectFaculties)
                                             .ThenInclude(x => x.SubjectFaculty)
                                             .Where(code => code.Hash == hash 
                                                         && !code.DateActivated.HasValue 
                                                         && !code.UserId.HasValue)
                                             .SingleOrDefaultAsync();

                if (one is null || one.MaxEndDate <= DateTime.Now.ToLocalTime())
                    return operation.SetFailed("Wrong code");

                one.UserId = userId;
                one.DateActivated = DateTime.Now.ToLocalTime();

                var subjectIds = one.CodePackages.SelectMany(x => x.Package.PackageSubjectFaculties.Select(x => x.SubjectFaculty.SubjectId));

                Context.AddRange(subjectIds.Except(await _query<SubjectFacultyAppUser>().Where(s => s.AppUserId == Context.CurrentUserId)
                                                                                        .Select(s => s.SubjectFacultyId)
                                                                                        .ToListAsync())
                                                                                        .Select(s => new SubjectFacultyAppUser
                                                                                        {
                                                                                            SubjectFacultyId = s,
                                                                                            AppUserId = Context.CurrentUserId.Value,
                                                                                        }));

                await Context.SaveChangesAsync();

                return operation.SetSuccess(true, $"Code {{{hash}}} Activated");
            };
     
        //private Func<OperationResult<bool>, Task<OperationResult<bool>>> _activateCodeV2(string hash, IEnumerable<Guid> subjectIds)
        //    => async operation => {

        //        var one = await TrackingQuery.Where(code => code.Hash == hash 
        //                                                 && !code.DateActivated.HasValue 
        //                                                 && !code.UserId.HasValue).SingleOrDefaultAsync();

        //        if (one is null || one.MaxEndDate <= DateTime.Now.ToLocalTime())
        //            return operation.SetFailed("Wrong code");

        //        if (one.Type == CodeTypes.Normal)
        //        {
        //            return await _activateCode(Context.CurrentUserId.Value, hash)(operation);
        //        }

        //        if ((!subjectIds?.Any()) ?? true)
        //            return operation.SetFailed($"Most set {nameof(subjectIds)}");

        //        if (subjectIds.Count() > 4 && one.Type == CodeTypes.Optional)
        //            return operation.SetFailed($"{nameof(subjectIds)} unvalid contain more 4 with Code Type {CodeTypes.Optional} value  {(int)CodeTypes.Optional} ");

        //        one.UserId = Context.CurrentUserId;
        //        one.DateActivated = DateTime.Now.ToLocalTime();

        //        var exams = await _query<Exam>().
        //               Where(exam => subjectIds.Contains(exam.SubjectId)).Select(exam => new { exam.Id, exam.Price }).ToListAsync();

        //        one.CodePackages = new List<CodePackage>() {
        //             new CodePackage(){
        //                 Package = new Package(){
        //                    Price = one.Value,
        //                    EndDate = one.MaxEndDate,
        //                    StartDate  = ExtensionMethodsShared.StartAcademicYear(),
        //                    Description = "مولد من قبل المستخدم",
        //                    Type = PackageTypes.Auto,
        //                    Name = $"حزمة {one.Type.GetDisplayDescription()}",
        //                    PackageExams = exams.Select(x=> new PackageExam(){
        //                            Price = x.Price,
        //                            ExamId = x.Id,
        //                    }).ToList()
        //                 }
        //             },
        //        };

        //        Context.AddRange(subjectIds.Except(await _query<SubjectAppUser>().Where(s => s.AppUserId == Context.CurrentUserId).Select(s => s.SubjectId).ToListAsync()).
        //            Select(s => new SubjectAppUser
        //            {
        //                SubjectId = s,
        //                AppUserId = Context.CurrentUserId.Value,
        //            }));

        //        await Context.SaveChangesAsync();

        //        return operation.SetSuccess(true, $"Code {{{hash}}} Activated");
        //    };

        //private Func<OperationResult<object>, Task<OperationResult<object>>> _checkCode(string hash)
        //=> async operation => {

        //    var one = await Query.Include(w => w.CodePackages).
        //    Where(code => code.Hash == hash && !code.DateActivated.HasValue && !code.UserId.HasValue).Select(code =>
        //        new CodeDescriptionDto
        //        {
        //            Hash = code.Hash,
        //            Type = code.Type,
        //            MaxEndDate = code.MaxEndDate,
        //            Value = code.Value,
        //            DiscountRate = code.DiscountRate,
        //            Description = code.CodePackages.Select(x => x.Package.Name).FirstOrDefault(),
        //            //Description =  code.Type == CodeTypes.Const? _query<Package>().FirstOrDefault(w=>w.Name.Equals("شاملة لسنة") ).Description : code.Type == CodeTypes.Optional ?
        //            //    _query<Package>().FirstOrDefault(w => w.Name.Equals("شاملة لسنة مع أربع مواد")).Description
        //            //    : _query<Package>().FirstOrDefault(w => w.Name.Equals("أربع مواد")).Description
        //        }
        //    ).FirstOrDefaultAsync();

        //    if (one is null || one.MaxEndDate <= DateTime.Now.ToLocalTime())
        //        return operation.SetFailed("Wrong code");

        //    one.Description = (one.Type == CodeTypes.Normal).NestedIF(one.Description, one.Type.GetDisplayDescription());

        //    return operation.SetSuccess(one);
        //};


       

       

        //private Func<OperationResult<CodeDetailsDto>, Task<OperationResult<CodeDetailsDto>>> _generate(CodeGenerateDto dto)
        // => async operation =>
        // {

        //     if (dto.SellerId == Guid.Empty)
        //     {
        //         //return operation.SetFailed($"It must init a {nameof(dto.SellerId)}");
        //         //{E85831D6-169A-466E-BBFC-9A1A118564A7}
        //         dto.SellerId = GlobalConstValues.DefaultSellerId;
        //     }

        //     if (dto.PackageIds is null || !dto.PackageIds.Any())
        //         return operation.SetFailed($"It must contain a {nameof(dto.PackageIds)}");

        //     var sellerName = await _query<AppUser>().Where(seller => seller.Id == dto.SellerId && seller.Type == UserTypes.Seller &&
        //        !seller.DateBlocked.HasValue && !seller.DateDeleted.HasValue).Select(seller => seller.Name).FirstOrDefaultAsync();
        //     if (sellerName.IsNullOrEmpty())
        //         return (OperationResultTypes.NotExist, "Sellers not match any SellerId");


        //     Code code = new Code();
        //     List<Package> packages = new List<Package>();
        //     bool isNormal = true;
        //     CodeTypes codeType = CodeTypes.Normal;

        //     dto.PackageIds = dto.PackageIds.Distinct();


        //     if (dto.PackageIds.Count() == 1)
        //     {
        //         var packageId = dto.PackageIds.Single();

        //         if (packageId == GlobalConstValues.DefaultConstPackageId)
        //             codeType = CodeTypes.Const;

        //         if (packageId == GlobalConstValues.DefaultConstOptionalPackageId)
        //             codeType = CodeTypes.ConstOptional;

        //         if (packageId == GlobalConstValues.DefaultOptionalPackageId)
        //             codeType = CodeTypes.Optional;

        //         if (codeType != CodeTypes.Normal)
        //         {
        //             var package = await _query<Package>().FirstOrDefaultAsync(p => p.Id == packageId);
        //             packages.Add(package);
        //             isNormal = false;
        //         }
        //     }

        //     if (isNormal)
        //     {
        //         DateTime dateNow = DateTime.Now.ToLocalTime();
        //         packages = await _query<Package>().Where(package => dto.PackageIds.Contains(package.Id) && !package.IsHidden && package.StartDate <= dateNow && package.EndDate >= dateNow).ToListAsync();
        //         if (packages.Count != dto.PackageIds.Count())
        //             return (OperationResultTypes.NotExist, "Once or more packages is not available anymore");
        //     }


        //     var _maxEndDate = packages.Max(package => package.EndDate);
        //     //if(_maxEndDate > dto.MaxEndDate)
        //     //    return operation.SetFailed($"You must enter {nameof(dto.MaxEndDate)} equal or max {_maxEndDate}");


        //     var _price = packages.Sum(package => package.Price);


        //     code = new Code()
        //     {
        //         DiscountRate = dto.DiscountRate,
        //         SellerId = dto.SellerId,
        //         Value = _price,
        //         MaxEndDate = ExtensionMethodsShared.EndAcademicYear(),
        //         Type = codeType,
        //     };

        //     if (isNormal)
        //     {
        //         code.MaxEndDate = _maxEndDate;
        //         code.CodePackages = packages.Select(package => new CodePackage()
        //         {
        //             PackageId = package.Id,
        //         }).ToList();
        //     }

        //     await ExtensionMethodsShared.SolveCannotInsertDuplicateKeyUniqueIndexAsync(async () => {
        //         code.Hash = ExtensionMethodsShared.GetUniqueKey(6);
        //         await Context.AddRangeAsync(code);
        //         await Context.SaveChangesAsync();
        //     });

        //     return operation.SetSuccess(new CodeDetailsDto
        //     {
        //         Id = code.Id,
        //         Value = code.Value,
        //         Hash = code.Hash,
        //         MaxEndDate = code.MaxEndDate,
        //         Type = code.Type,
        //         DateActivated = code.DateActivated,
        //         DiscountRate = code.DiscountRate,
        //         SellerId = code.SellerId,
        //         SellerName = sellerName,//name
        //         UserId = code.UserId,
        //         UserName = code.UserId.HasValue ? code.User.Name : default,
        //         PaidValue = code.InvoiceId.HasValue ? code.Invoice.Total : default,
        //         DateCreated = code.DateCreated,
        //         PackageName = packages.FirstOrDefault().Name,
        //     });
        // };

        //private Func<OperationResult<IEnumerable<CodeDetailsDto>>, Task<OperationResult<IEnumerable<CodeDetailsDto>>>> _getAll()
        //=> async operation => {

        //    var list = await Query.Select(code => new CodeDetailsDto
        //    {
        //        Id = code.Id,
        //        Value = code.Value,
        //        Hash = code.Hash,
        //        MaxEndDate = code.MaxEndDate,
        //        DateActivated = code.DateActivated,
        //        Type = code.Type,
        //        DiscountRate = code.DiscountRate,
        //        SellerId = code.SellerId,
        //        SellerName = code.Seller.Name,
        //        UserId = code.UserId,
        //        UserName = code.UserId.HasValue ? code.User.Name : default,
        //        PaidValue = code.Value,
        //        PackageName = code.CodePackages.Select(p => p.Package.Name).FirstOrDefault(),
        //        DateCreated = code.DateCreated,
        //    }).ToListAsync();

        //    return operation.SetSuccess(list.OrderByDescending(w => w.DateCreated).ToList());
        //};

        //private Func<OperationResult<bool>, Task<OperationResult<bool>>> _delete(Guid id)
        //=> async operation => {

        //    var one = await FindAsync(id);
        //    if (one is null)
        //        return (OperationResultTypes.NotExist, $"{id} not exist");

        //    await Context.SoftDeleteTraversalAsync<Code, CodePackage>(p => p.Id == id, p => p.CodePackages);
        //    await Context.SaveChangesDeletedAsync();

        //    return operation.SetSuccess(true, "Success delete");
        //};

        //private Func<OperationResult<CodeDetailsDto>, Task<OperationResult<CodeDetailsDto>>> _update(CodeGenerateDto dto)
        //=> async operation =>
        //{

        //    var code = await FindAsync(dto.Id);
        //    if (code is null)
        //        return (OperationResultTypes.NotExist, $"{dto.Id} not exist");


        //    if (dto.SellerId == Guid.Empty)
        //        return operation.SetFailed($"It must init a {nameof(dto.SellerId)}");

        //    if (dto.PackageIds is null || !dto.PackageIds.Any())
        //        return operation.SetFailed($"It must contain a {nameof(dto.PackageIds)}");

        //    dto.PackageIds = dto.PackageIds.Distinct();

        //    DateTime dateNow = DateTime.Now.ToLocalTime();
        //    var packages = await _query<Package>().Where(package => dto.PackageIds.Contains(package.Id) && !package.IsHidden && package.StartDate <= dateNow && package.EndDate >= dateNow).ToListAsync();
        //    if (packages.Count != dto.PackageIds.Count())
        //        return (OperationResultTypes.NotExist, "Once or more packages is not available anymore");


        //    var _maxEndDate = packages.Max(package => package.EndDate);

        //    var _price = packages.Sum(package => package.Price);


        //    var sellerName = await _query<AppUser>().Where(seller => seller.Id == dto.SellerId && seller.Type == UserTypes.Seller &&
        //     !seller.DateBlocked.HasValue && !seller.DateDeleted.HasValue).Select(seller => seller.Name).FirstOrDefaultAsync();
        //    if (sellerName.IsNullOrEmpty())
        //        return (OperationResultTypes.NotExist, "Sellers not match any SellerId");


        //    //reset all old package
        //    Context.RemoveRange(await Context.CodePackages.Where(c => c.CodeId == code.Id).ToListAsync());

        //    code.DiscountRate = dto.DiscountRate;
        //    code.SellerId = dto.SellerId;
        //    code.Value = _price;
        //    code.MaxEndDate = _maxEndDate;
        //    code.CodePackages = packages.Select(package => new CodePackage()
        //    {
        //        PackageId = package.Id,
        //    }).ToList();

        //    await Context.SaveChangesAsync();


        //    return operation.SetSuccess(new CodeDetailsDto
        //    {
        //        Id = code.Id,
        //        Value = code.Value,
        //        Hash = code.Hash,
        //        Type = code.Type,
        //        MaxEndDate = code.MaxEndDate,
        //        DateActivated = code.DateActivated,
        //        DiscountRate = code.DiscountRate,
        //        SellerId = code.SellerId,
        //        SellerName = sellerName,//name
        //        UserId = code.UserId,
        //        UserName = code.UserId.HasValue ? code.User.Name : default,
        //        PaidValue = code.InvoiceId.HasValue ? code.Invoice.Total : default,
        //        DateCreated = code.DateCreated,
        //        PackageName = packages.FirstOrDefault().Name,
        //    });
        //};


    }
}
