using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Helper.ExtensionMethods.Boolean;
using Elkood.Web.Helper.ExtensionMethods.String;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SmartStart.DataTransferObject.AccountDto;
using SmartStart.DataTransferObject.UserDto;
using SmartStart.Model.Security;
using SmartStart.SharedKernel.Enums;
using SmartStart.SharedKernel.ExtensionMethods;
using SmartStart.SharedKernel.Security;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Security.UserService
{
    [ElRepository]
    public class UserRepository : ElRepository<SmartStartDbContext, Guid, AppUser>, IUserRepository
    {
        public UserManager<AppUser> UserManager { get; }
        private readonly IDistributedCache Cache;

        public UserRepository(SmartStartDbContext context, UserManager<AppUser> userManager, IDistributedCache cache) : base(context)
        {
            UserManager = userManager;
            Cache = cache;
        }

        public async Task<OperationResult<IEnumerable<AppUserDto>>> GetAllUser()
           => await RepositoryHandler(_getAllUser());

        public async Task<OperationResult<AppUserDto>> Create(AppUserDto appsuer)
            => await RepositoryHandler(_create(appsuer));

        public async Task<OperationResult<AppUserDto>> Update(AppUserDto account)
          => await RepositoryHandler(_update(account));

        public async Task<OperationResult<bool>> Delete(Guid id)
         => await RepositoryHandler(_delete(id));

        public async Task<OperationResult<bool>> DeleteRange(IEnumerable<Guid> ids)
           => await RepositoryHandler(_deleteRange(ids));

        public async Task<OperationResult<bool>> Block(Guid id)
        => await RepositoryHandler(_block(id));

        public async Task<OperationResult<UserDetailsDto>> Details(Guid id)
        => await RepositoryHandler(_details(id));



        private Func<OperationResult<UserDetailsDto>, Task<OperationResult<UserDetailsDto>>> _details(Guid id)
        => async operation => {
            var one = await Query.Include(w => w.UserCodes).ThenInclude(w => w.CodePackages).Where(user => user.Id == id && !user.DateDeleted.HasValue && user.Type == UserTypes.User).
           Select(user => new UserDetailsDto()
           {
               Id = user.Id,
               UserName = user.UserName,
               Email = user.Email,
               Name = user.Name,
               PhoneNumber = user.PhoneNumber,
               Birthday = user.Birthday,
               Address = user.Address,
               Gender = user.Gender,
               SubscriptionDate = user.SubscriptionDate,
               DateBlocked = user.DateBlocked,
               DateActivated = user.DateActivated,
               Type = user.Type,
               FacultyId = user.FacultyId,
               SubscriptionCount = user.UserCodes.Count(),
               Codes = user.UserCodes.Select(code => new UserCodeDto()
               {
                   Id = code.Id,
                   Hash = code.Hash,
                   Value = code.Value,
                   DateActivated = code.DateActivated,
                   DiscountRate = code.DiscountRate,
                   MaxEndDate = code.MaxEndDate,
                   SellerName = code.Seller.Name,
                   Package = code.CodePackages.Select(package => new UserPackageDto()
                   {
                       Id = package.Package.Id,
                       Name = package.Package.Name,
                       Description = package.Package.Description,
                       Price = package.Package.Price,
                       EndDate = package.Package.EndDate,
                       StartDate = package.Package.StartDate,
                       Type = package.Package.Type,
                   }).FirstOrDefault()
               }),
           }).FirstOrDefaultAsync();

            if (one is null)
                return OperationResultTypes.NotExist;

            return operation.SetSuccess(one);
        };

        private Func<OperationResult<AppUserDto>, Task<OperationResult<AppUserDto>>> _update(AppUserDto account)
          => async operation =>
          {
              AppUser User = await UserManager.FindByIdAsync(account.Id.ToString());

              if (User is null || User.Type != UserTypes.User)
                  return OperationResultTypes.NotExist;

              User.UserName = account.UserName;
              User.Email = account.Email;
              User.Name = account.Name;
              User.PhoneNumber = account.PhoneNumber;
              User.Birthday = account.Birthday;
              User.Address = account.Address;
              User.Gender = account.Gender;
              User.DateBlocked = account.DateBlocked;
              User.DateActivated = account.DateActivated;
              User.FacultyId = account.FacultyId;
              User.SubscriptionDate = account.SubscriptionDate;
              User.Type = UserTypes.User;

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
               if (User is null || User.Type != UserTypes.User)
                   return OperationResultTypes.NotExist;

               Context.SoftDelete(User);
               IdentityResult identityResult = await UserManager.UpdateAsync(User);
               if (!identityResult.Succeeded)
                   return operation.SetFailed(String.Join(",", identityResult.Errors.Select(error => error.Description)));

               await PushUserInBlacklist(User);

               return operation.SetSuccess(true);
           };

        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _deleteRange(IEnumerable<Guid> ids)
           => async operation =>
           {
               //AppUser User = await UserManager.FindByIdAsync(id.ToString());
               //if (User is null || User.Type != UserTypes.User)
               //    return OperationResultTypes.NotExist;

               ids.ToList().ForEach(id =>
               {
                   Context.SoftDelete<AppUser>(id);
               });


               await Context.SaveChangesAsync(); ;

               //Context.SoftDelete(User);
               //IdentityResult identityResult = await UserManager.UpdateAsync(User);
               //if (!identityResult.Succeeded)
               //    return operation.SetFailed(String.Join(",", identityResult.Errors.Select(error => error.Description)));

               //await PushUserInBlacklist(User);

               return operation.SetSuccess(true);
           };


        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _block(Guid id)
        => async operation =>
        {
            AppUser User = await UserManager.FindByIdAsync(id.ToString());
            if (User is null || User.Type != UserTypes.User)
                return OperationResultTypes.NotExist;

            User.DateBlocked = User.DateBlocked.HasValue ? (DateTime?)null : DateTime.Now.ToLocalTime();
            IdentityResult identityResult = await UserManager.UpdateAsync(User);
            if (!identityResult.Succeeded)
                return operation.SetFailed(String.Join(",", identityResult.Errors.Select(error => error.Description)));

            await PushUserInBlacklist(User);

            return operation.SetSuccess(true, User.DateBlocked.HasValue ? "blocked" : "unblocked");
        };

        private Func<OperationResult<AppUserDto>, Task<OperationResult<AppUserDto>>> _create(AppUserDto account)
            => async operation => {

                AppUser user = new AppUser()
                {
                    UserName = account.PhoneNumber,
                    Email = account.Email.IsNullOrEmpty().NestedIF(account.PhoneNumber.ToEmail(), account.Email.EnsureEmail()),
                    Name = account.Name,
                    PhoneNumber = account.PhoneNumber,
                    Birthday = account.Birthday,
                    Address = account.Address,
                    Gender = account.Gender,
                    //FacultyId = account.FacultyId,
                    SubscriptionDate = account.SubscriptionDate,
                    Type = UserTypes.User,
                    DateActivated = account.DateActivated,
                    //DateBlocked
                };

                var identityResult = await UserManager.CreateAsync(user, account.Password);

                if (!identityResult.Succeeded)
                    return operation.SetFailed(String.Join(",", identityResult.Errors.Select(error => error.Description)));

                var roleIdentityResult = await UserManager.AddToRoleAsync(user, SmartStartRoles.User.ToString());

                if (!roleIdentityResult.Succeeded)
                    return operation.SetFailed(String.Join(",", roleIdentityResult.Errors.Select(error => error.Description)));

                account.Id = user.Id;

                return operation.SetSuccess(account);
            };

        private Func<OperationResult<IEnumerable<AppUserDto>>, Task<OperationResult<IEnumerable<AppUserDto>>>> _getAllUser()
            => async operation => {

                var AllQyery = _query<AppUser>().Include(f => f.UserCodes)
                                                 .Include(x => x.SubjectFacultyAppUsers)
                                                 .ThenInclude(x => x.SubjectFaculty)
                                                 //.ThenInclude(x => x.Faculties)
                                                 .Where(user => user.Type == UserTypes.User);


                //var UserFaculties = AllQyery.Select(user => new UserFacultyDto
                //{
                //    Id = user.Id,
                //    Faculties = user.SubjectAppUsers.Select(e => e.SubjectFacultyId).Distinct().ToList()
                //}).ToList();




                //Dictionary<Guid, List<Guid>> dic = new Dictionary<Guid, List<Guid>>();

                //for (int i = 0; i < UserFaculties.Count(); i++)
                //{
                //    dic[UserFaculties[i].Id] = UserFaculties[i].Faculties.Distinct().ToList();
                //}

                var list = await AllQyery.OrderByDescending(w => w.DateCreated)
                           .Select(user => new AppUserDto
                           {
                               Id = user.Id,
                               Name = user.Name,
                               Address = user.Address,
                               Birthday = user.Birthday,
                               DateActivated = user.DateActivated,
                               DateBlocked = user.DateBlocked,
                               DateCreated = user.DateCreated,
                               Email = user.Email,
                               Gender = user.Gender,
                               PhoneNumber = user.PhoneNumber,
                               UserName = user.UserName,
                               Type = user.Type,
                               SubscriptionDate = user.SubscriptionDate,
                               SubscriptionCount = user.UserCodes.Count(),
                               FacultiesIds = user.SubjectFacultyAppUsers.Select(e => e.SubjectFaculty.FacultyId).ToList()
                           })
                           .ToListAsync();

                return operation.SetSuccess(list);
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
