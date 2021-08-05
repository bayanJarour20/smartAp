using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SmartStart.Model.Business;
using SmartStart.Model.Security;
using SmartStart.Repository.PointOfSale.DataTransferObject;
using SmartStart.Repository.PointOfSale.PointOfSaleService.DataTransferObject;
using SmartStart.SharedKernel.Enums;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.PointOfSale.PointOfSaleService
{
    [ElRepository]
    public class PointOfSaleRepository : ElRepository<SmartStartDbContext, Guid, AppUser>, IPointOfSaleRepository
    {
        public UserManager<AppUser> UserManager { get; }
        private readonly IDistributedCache Cache;

        public PointOfSaleRepository(SmartStartDbContext context, UserManager<AppUser> userManager, IDistributedCache cache) : base(context)
        {
            UserManager = userManager;
            Cache = cache;
        }

        public async Task<OperationResult<PointOfSaleDto>> Fetch(Guid id)
            => await RepositoryHandler(_fetch(id));

        public async Task<OperationResult<IEnumerable<PackageDto>>> GetPackages(Guid UserId)
           => await RepositoryHandler(_pacakges(UserId));

        public async Task<OperationResult<CodePackagesDto>> GenerateCode(Guid id, GenerateCodeDto generateCode)
           => await RepositoryHandler(_generateCode(id, generateCode));

        public async Task<OperationResult<NotificationCollectionDto>> GetNotifications()
        => await RepositoryHandler(_getNotifications);

        public async Task<OperationResult<PointOfSaleAccountDto>> Create(PointOfSaleAccountDto account)
        => await RepositoryHandler(_create(account));

        public async Task<OperationResult<PointOfSaleAccountDto>> Update(PointOfSaleAccountDto account)
           => await RepositoryHandler(_update(account));

        public async Task<OperationResult<bool>> Delete(Guid id)
         => await RepositoryHandler(_delete(id));

        public async Task<OperationResult<bool>> Block(Guid id)
        => await RepositoryHandler(_block(id));

        public async Task<OperationResult<IEnumerable<PointOfSaleAccountDto>>> GetAll()
          => await RepositoryHandler(_getAll());

        public async Task<OperationResult<PointOfSaleAccountCodesDto>> Details(Guid id)
         => await RepositoryHandler(_details(id));

        public async Task<OperationResult<object>> GetPointOfSales()
         => await RepositoryHandler(_getPointOfSales());



        private Func<OperationResult<object>, Task<OperationResult<object>>> _getPointOfSales()
        => async operation => {

            var list = await Query.Where(user => !user.DateDeleted.HasValue && user.Type == UserTypes.Seller && !user.DateDeleted.HasValue && user.DateActivated.HasValue).
                            Select(user => new
                            {
                                Name = user.Name,
                                PhoneNumber = user.PhoneNumber,
                                Address = user.Address,
                                CityName = user.City.Name
                            }).ToListAsync();

            return operation.SetSuccess(list);
        };

        private Func<OperationResult<PointOfSaleDto>, Task<OperationResult<PointOfSaleDto>>> _fetch(Guid id)
        => async operation =>
        {
            var result = await Query.Where(user => user.Id == id && !user.DateDeleted.HasValue && user.Type == UserTypes.Seller).
            Select(user => new PointOfSaleDto
            {
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                MoneyLimit = user.MoneyLimit,
                AllowDiscount = user.AllowDiscount,
                CityId = user.CityId ?? default,
                Rate = user.Rates.OrderByDescending(rate => rate.DateCreated).Select(rate => rate.DiscountRate * 100).FirstOrDefault(),
                Codes = user.Codes.
                        Select(code => new CodePackagesDto
                        {
                            Hash = code.Hash,
                            Value = code.Value,
                            MaxEndDate = code.MaxEndDate,
                            DiscountRate = code.DiscountRate,
                            DateCreated = code.DateCreated,
                            DateActivated = code.DateActivated,
                            IsInvoice = code.InvoiceId.HasValue,
                            Packages = code.CodePackages.
                               Select(codepackage => new PackageDto
                               {
                                   Id = codepackage.Package.Id,
                                   Name = codepackage.Package.Name,
                                   Description = codepackage.Package.Description,
                                   Price = codepackage.Package.Price,
                                   StartDate = codepackage.Package.StartDate,
                                   EndDate = codepackage.Package.EndDate,
                                   Type = codepackage.Package.Type,
                               }),
                        }).OrderByDescending(code => code.DateCreated).ToList(),
                Invoices = user.Invoices.Select(invoice => new InvoiceDto
                {
                    SerialNumber = invoice.SerialNumber,
                    Note = invoice.Note,
                    Total = invoice.Total,
                    Date = invoice.Date,
                    Rate = invoice.Rate,
                }),
            }).FirstOrDefaultAsync();

            foreach (var code in result.Codes)
            {
                code.Packages ??= new List<PackageDto>() {
                    new PackageDto()
                    {
                        Name = $"حزمة {code.Type.GetDisplayDescription()}",
                        Description = "",
                        Price = code.Value,
                        StartDate =code.DateCreated,
                        EndDate = code.MaxEndDate,
                    }
                };
            }


            return operation.SetSuccess(result);
        };

        private Func<OperationResult<IEnumerable<PackageDto>>, Task<OperationResult<IEnumerable<PackageDto>>>> _pacakges(Guid userId)
            => async operation =>
            {
                var defaultquery = _query<Package>().Where(package =>  !package.IsHidden 
                                                        && package.StartDate <= DateTime.Now 
                                                        && package.EndDate >= DateTime.Now);

                if (_query<>().Any(w => w.AppUserId == userId))
                    defaultquery.Where(package => package.PackageExams.Any(w => w.Exam.Subject.Faculty.PosUsers.Any(w => w.AppUserId == userId)));

                var result = await defaultquery.Select(Package => new PackageDto
                {
                    Id = Package.Id,
                    Name = Package.Name,
                    Description = Package.Description,
                    Price = Package.Price,
                    StartDate = Package.StartDate,
                    EndDate = Package.EndDate,
                    Type = Package.Type
                }).ToListAsync();

                return operation.SetSuccess(result);
            };

        private Func<OperationResult<CodePackagesDto>, Task<OperationResult<CodePackagesDto>>> _generateCode(Guid id, GenerateCodeDto generateCode)
        => async operation =>
        {
            if (generateCode.PackageIds is null || !generateCode.PackageIds.Any())
                return operation.SetFailed("it must contain a package");


            var userMoney = await Query.Where(user => user.Id == id && !user.DateDeleted.HasValue).
               Select(user => new
               {
                   user.Id,
                   user.MoneyLimit,
                   UnInvoiceMoney = user.Codes.Where(code => !code.InvoiceId.HasValue).Sum(code => code.Value),
                   DiscountRate = user.Rates.OrderByDescending(rate => rate.DateCreated).Select(rate => rate.DiscountRate).FirstOrDefault(),
               }).FirstOrDefaultAsync();

            if (userMoney is null)
                return (OperationResultTypes.NotExist, "seller not exist");


            Code code = new Code();
            List<Package> packages = new List<Package>();
            bool isNormal = true;
            CodeTypes codeType = CodeTypes.Normal;

            generateCode.PackageIds = generateCode.PackageIds.Distinct();

            if (generateCode.PackageIds.Count() == 1)
            {
                var packageId = generateCode.PackageIds.Single();

                if (packageId == GlobalConstValues.DefaultConstPackageId)
                    codeType = CodeTypes.Const;

                if (packageId == GlobalConstValues.DefaultConstOptionalPackageId)
                    codeType = CodeTypes.ConstOptional;

                if (packageId == GlobalConstValues.DefaultOptionalPackageId)
                    codeType = CodeTypes.Optional;

                if (codeType != CodeTypes.Normal)
                {
                    var package = await _query<Package>().FirstOrDefaultAsync(p => p.Id == packageId);
                    packages.Add(package);
                    isNormal = false;
                }
            }

            if (isNormal)
            {
                DateTime dateNow = DateTime.Now.ToLocalTime();
                packages = await _query<Package>().Where(package => generateCode.PackageIds.Contains(package.Id) && !package.IsHidden && package.StartDate <= dateNow && package.EndDate >= dateNow).ToListAsync();
                if (packages.Count != generateCode.PackageIds.Count())
                    return (OperationResultTypes.NotExist, "once or more packages is not available anymore");
            }


            if (userMoney.MoneyLimit.HasValue)
                if (userMoney.MoneyLimit <= (userMoney.UnInvoiceMoney + packages.Sum(package => package.Price)))
                    return operation.SetFailed("allowed money limit exceeded");

            var maxEndDate = packages.Max(package => package.EndDate);
            var price = packages.Sum(package => package.Price);


            code = new Code()
            {
                DiscountRate = generateCode.DiscountRate,
                SellerId = id,
                Value = price,
                MaxEndDate = ExtensionMethodsShared.EndAcademicYear(),
                Type = codeType,
            };

            if (isNormal)
            {
                code.MaxEndDate = maxEndDate;
                code.CodePackages = packages.Select(package => new CodePackage()
                {
                    PackageId = package.Id,
                }).ToList();
            }

            await ExtensionMethodsShared.SolveCannotInsertDuplicateKeyUniqueIndexAsync(async () => {
                code.Hash = ExtensionMethodsShared.GetUniqueKey(6);
                await Context.AddRangeAsync(code);
                await Context.SaveChangesAsync();
            });

            return operation.SetSuccess(new CodePackagesDto
            {
                Hash = code.Hash,
                Value = code.Value,
                DiscountRate = code.DiscountRate,
                DateActivated = code.DateActivated,
                DateCreated = code.DateCreated,
                IsInvoice = code.InvoiceId.HasValue,
                MaxEndDate = code.MaxEndDate,
                Packages = packages.Select(p => new PackageDto()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Type = p.Type,
                }),
            });
        };

        private Func<OperationResult<NotificationCollectionDto>, Task<OperationResult<NotificationCollectionDto>>> _getNotifications
            => async operation => {

                Collection<NotificationDto> notificationToday = new Collection<NotificationDto>();
                Collection<NotificationDto> notification = new Collection<NotificationDto>();
                DateTime date_now = DateTime.Today;

                (await _query<Notification>()
                .Where(notification => notification.Type == NotificationTypes.Seller).
                Select(notification => new NotificationDto
                {
                    Title = notification.Title,
                    Body = notification.Body,
                    Date = notification.Date,
                    HasReaded = notification.HasReaded,
                }).ToListAsync()).ForEach(_notification => {
                    if (_notification.Date.Date == date_now)
                        notificationToday.Add(_notification);
                    else notification.Add(_notification);
                });

                return operation.SetSuccess(new NotificationCollectionDto
                {
                    NotificationToday = notificationToday,
                    Notification = notification
                });
            };



        private Func<OperationResult<PointOfSaleAccountDto>, Task<OperationResult<PointOfSaleAccountDto>>> _create(PointOfSaleAccountDto account)
        => async operation => {

            AppUser user = new AppUser()
            {
                UserName = account.UserName,
                Email = account.Email,
                Name = account.Name,
                PhoneNumber = account.PhoneNumber,
                Birthday = account.Birthday,
                Address = account.Address,
                Gender = account.Gender,
                MoneyLimit = account.MoneyLimit,
                SubscriptionDate = account.SubscriptionDate,
                AllowDiscount = account.AllowDiscount,
                Type = UserTypes.Seller,
                CityId = account.CityId,
                DateActivated = account.DateActivated,
                Faculties = account.facList?.Distinct()?.Select(id => new FacultyPOSUser()
                {
                    FacultyId = id
                }).ToList(),
            };

            var identityResult = await UserManager.CreateAsync(user, account.Password);

            if (!identityResult.Succeeded)
                return operation.SetFailed(String.Join(",", identityResult.Errors.Select(error => error.Description)));

            var roleIdentityResult = await UserManager.AddToRoleAsync(user, TarafouaRoles.Seller.ToString());

            if (!roleIdentityResult.Succeeded)
                return operation.SetFailed(String.Join(",", roleIdentityResult.Errors.Select(error => error.Description)));

            account.Id = user.Id;
            await Context.Rates.AddAsync(new Rate
            {
                DateCreated = DateTime.Now,
                DiscountRate = account.Rate,
                SellerId = user.Id
            });
            await Context.SaveChangesAsync();
            return operation.SetSuccess(account);
        };

        private Func<OperationResult<PointOfSaleAccountDto>, Task<OperationResult<PointOfSaleAccountDto>>> _update(PointOfSaleAccountDto account)
        => async operation =>
        {
            AppUser User = await UserManager.FindByIdAsync(account.Id.ToString());

            if (User is null || User.Type != UserTypes.Seller)
                return OperationResultTypes.NotExist;

            User.UserName = account.UserName;
            User.Email = account.Email;
            User.Name = account.Name;
            User.PhoneNumber = account.PhoneNumber;
            User.Birthday = account.Birthday;
            User.Address = account.Address;
            User.Gender = account.Gender;
            User.DateBlocked = account.DateBlocked;
            User.MoneyLimit = account.MoneyLimit;
            User.SubscriptionDate = account.SubscriptionDate;
            User.AllowDiscount = account.AllowDiscount;
            User.Type = UserTypes.Seller;
            User.CityId = account.CityId;
            User.DateActivated = account.DateActivated;
            User.Faculties = account.facList?.Distinct().Select(id => new FacultyPOSUser()
            {
                FacultyId = id
            }).ToList();

            // ensure remove all old faculty 
            Context.RemoveRange(Context.FacultyPOSUsers.Where(x => x.AppUserId == account.Id).ToList());


            var rate = (await TrackingQuery.Where(user => user.Id == account.Id)
                                           .Include(user => user.Rates)
                                           .SingleOrDefaultAsync()).Rates.OrderByDescending(r => r.DateCreated);
            var currentTime = DateTime.Now;
            if (rate != null && rate.Any() && rate.First().DiscountRate != account.Rate)
            {
                rate.First().DateUpdated = currentTime;
                Context.Rates.Update(rate.First());
                Context.Rates.Add(new Rate
                {
                    DateCreated = currentTime,
                    DiscountRate = account.Rate,
                    SellerId = User.Id
                });
                await Context.SaveChangesAsync();
            }


            if (!account.Password.IsNullOrEmpty())
                User.PasswordHash = UserManager.PasswordHasher.HashPassword(User, account.Password);

            IdentityResult identityResult = await UserManager.UpdateAsync(User);
            if (!identityResult.Succeeded)
                return operation.SetFailed(String.Join(",", identityResult.Errors.Select(error => error.Description)));

            account.Password = string.Empty;

            await PushUserInBlacklist(User, true);

            return operation.SetSuccess(account);
        };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _delete(Guid id)
           => async operation =>
           {
               AppUser User = await UserManager.FindByIdAsync(id.ToString());
               if (User is null || User.Type != UserTypes.Seller)
                   return OperationResultTypes.NotExist;

               Context.SoftDelete(User);
               IdentityResult identityResult = await UserManager.UpdateAsync(User);
               if (!identityResult.Succeeded)
                   return operation.SetFailed(String.Join(",", identityResult.Errors.Select(error => error.Description)));

               await PushUserInBlacklist(User);

               return operation.SetSuccess(true);
           };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _block(Guid id)
        => async operation =>
        {
            AppUser User = await UserManager.FindByIdAsync(id.ToString());
            if (User is null || User.Type != UserTypes.Seller)
                return OperationResultTypes.NotExist;

            User.DateBlocked = User.DateBlocked.HasValue ? null : DateTime.Now.ToLocalTime();
            IdentityResult identityResult = await UserManager.UpdateAsync(User);
            if (!identityResult.Succeeded)
                return operation.SetFailed(String.Join(",", identityResult.Errors.Select(error => error.Description)));

            await PushUserInBlacklist(User);

            return operation.SetSuccess(true, User.DateBlocked.HasValue ? "blocked" : "unblocked");
        };

        private Func<OperationResult<IEnumerable<PointOfSaleAccountDto>>, Task<OperationResult<IEnumerable<PointOfSaleAccountDto>>>> _getAll()
        => async operation => {

            var list = await Query.Where(user => !user.DateDeleted.HasValue && user.Type == UserTypes.Seller).
            Select(user => new PointOfSaleAccountDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Birthday = user.Birthday,
                Address = user.Address,
                Gender = user.Gender,
                MoneyLimit = user.MoneyLimit,
                SubscriptionDate = user.SubscriptionDate,
                DateBlocked = user.DateBlocked,
                AllowDiscount = user.AllowDiscount,
                CodeSoldCount = user.Codes.Count(code => code.DateActivated.HasValue),
                CityId = user.CityId ?? default,
                DateActivated = user.DateActivated,
            }).ToListAsync();

            return operation.SetSuccess(list);
        };

        private Func<OperationResult<PointOfSaleAccountCodesDto>, Task<OperationResult<PointOfSaleAccountCodesDto>>> _details(Guid id)
        => async operation => {

            var one = await Query.Include(w => w.Codes).ThenInclude(w => w.CodePackages).Where(user => user.Id == id && !user.DateDeleted.HasValue && user.Type == UserTypes.Seller).
              Select(user => new PointOfSaleAccountCodesDto()
              {
                  Id = user.Id,
                  UserName = user.UserName,
                  Email = user.Email,
                  Name = user.Name,
                  PhoneNumber = user.PhoneNumber,
                  Birthday = user.Birthday,
                  Address = user.Address,
                  Gender = user.Gender,
                  MoneyLimit = user.MoneyLimit,
                  SubscriptionDate = user.SubscriptionDate,
                  DateBlocked = user.DateBlocked,
                  AllowDiscount = user.AllowDiscount,
                  CityId = user.CityId ?? default,
                  DateActivated = user.DateActivated,
                  facList = user.Faculties.Select(w => w.FacultyId),
                  Rate = user.Rates.OrderByDescending(r => r.DateCreated).Select(x => x.DiscountRate * 100).FirstOrDefault(),
                  // CodeSoldCount = user.Codes.Count(code => code.DateActivated.HasValue),
                  CodeDetailsSimpleDto = user.Codes.Select(code => new CodeDetailsSimpleDto()
                  {
                      Id = code.Id,
                      Hash = code.Hash,
                      Value = code.Value,
                      CreateDate = code.DateCreated,
                      DateActivated = code.DateActivated,
                      DiscountRate = code.DiscountRate,
                      MaxEndDate = code.MaxEndDate,
                      StudentName = code.User.Name,
                      Package = code.CodePackages.Select(package => new PackageDto()
                      {
                          Id = package.Package.Id,
                          Name = package.Package.Name,
                          Description = package.Package.Description,
                          Price = package.Package.Price,
                          EndDate = package.Package.EndDate,
                          StartDate = package.Package.StartDate,
                          Type = package.Package.Type

                      }).FirstOrDefault()
                  }),
              }).FirstOrDefaultAsync();

            if (one is null)
                return OperationResultTypes.NotExist;

            one.CodeSoldCount = one.CodeDetailsSimpleDto.Count(code => code.DateActivated.HasValue);

            return operation.SetSuccess(one);
        };



        const string DistributedLey_label = "tarafoua_user_blacklist:";

        private async Task PushUserInBlacklist(AppUser User, bool isUpdated = false)
        {
            ///TODO ElKood.Web
            //put user in black list until fill 'not date yet' or token expired
            if (User.DateBlocked.HasValue || User.DateDeleted.HasValue || isUpdated)
                await Cache.SetStringAsync(DistributedLey_label + User.Id, "-", new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(GlobalConstValues.DefaultExpireTokenMinut),
                    SlidingExpiration = TimeSpan.FromMinutes(GlobalConstValues.DefaultExpireTokenMinut)
                }); //set
            else
                await Cache.RemoveAsync(DistributedLey_label + User.Id);
        }   
    }
}
