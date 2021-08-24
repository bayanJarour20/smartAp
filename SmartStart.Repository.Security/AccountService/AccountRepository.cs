using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Helper.ExtensionMethods.Boolean;
using Elkood.Web.Helper.ExtensionMethods.String;
using Elkood.Web.Security.Claims;
using Elkood.Web.Service.BoundedContext;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartStart.DataTransferObject.AccountDto;
using SmartStart.Model.Security;
using SmartStart.SharedKernel.Enums;
using SmartStart.SharedKernel.ExtensionMethods;
using SmartStart.SharedKernel.Security;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Security.AccountService
{
    [ElRepository]
    public class AccountRepository : ElRepository<SmartStartDbContext, Guid>, IAccountRepository
    {
        public UserManager<AppUser> UserManager { get; }
        public SignInManager<AppUser> SignInManager { get; }

        private readonly IConfiguration Config;
        private readonly IDistributedCache Cache;

        public AccountRepository(SmartStartDbContext context,
                            UserManager<AppUser> userManager,
                            SignInManager<AppUser> signInManager,
                            IConfiguration config, IDistributedCache cache) : base(context)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Config = config;
            Cache = cache;
        }
        public async Task<OperationResult<AccountDto>> Login(LoginDto loginDto)
           => await RepositoryHandler(_login(loginDto));
        public async Task<OperationResult<AccountDto>> Signin(SigninDto signinDto)
            => await RepositoryHandler(_signin(signinDto));
        public async Task<OperationResult<AccountDto>> RefreshToken(string id, string refreshToken, string generationStamp)
            => await RepositoryHandler(_refreshToken(id, refreshToken, generationStamp));
        public async Task<OperationResult<object>> Signup(SignupDto signupDto)
            => await RepositoryHandler(_signup(signupDto));
        public async Task<OperationResult<IEnumerable<AppUserDto>>> GetAllDashboard()
            => await RepositoryHandler(_getAllDashboard());
        public async Task<OperationResult<AppUserDto>> Create(AppUserDto appsuer)
            => await RepositoryHandler(_create(appsuer));
        public async Task<OperationResult<AppUserDto>> Update(AppUserDto account)
            => await RepositoryHandler(_update(account));
        public async Task<OperationResult<bool>> Delete(Guid id)
            => await RepositoryHandler(_delete(id));
        public async Task<OperationResult<bool>> Block(Guid id)
            => await RepositoryHandler(_block(id));
        public async Task<OperationResult<AppUserDto>> Details(Guid id)
            => await RepositoryHandler(_details(id));
        public async Task<OperationResult<AccountDto>> Edit(Guid id, SignupDto dto, string oldPassword)
            => await RepositoryHandler(_edit(id, dto, oldPassword));

        private Func<OperationResult<AccountDto>, Task<OperationResult<AccountDto>>> _login(LoginDto loginDto)
            => async operation =>
            {
                var user = await UserManager.FindByNameAsync(loginDto.Username);

                if (user == null || user.DateDeleted.HasValue || user.Type != UserTypes.Dashboard)
                    return operation.SetFailed("login failed", OperationResultTypes.Forbidden);

                if (user.DateBlocked.HasValue)
                    return operation.SetFailed("blocked", OperationResultTypes.Forbidden);

                var loginResult = await SignInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if (loginResult == SignInResult.Success)
                {
                    AccountDto accountDto = await FillAccount(user, true);
                    AssignRefreshTokenIfRememberMe(loginDto.RememberMe, accountDto);

                    await TemporaryFillingInBlacklist(user);

                    await TemporaryGenerationStamp(user);

                    return operation.SetSuccess(accountDto);
                }

                return operation.SetFailed("login failed", OperationResultTypes.Forbidden);
            };
        private Func<OperationResult<AccountDto>, Task<OperationResult<AccountDto>>> _signin(SigninDto signinDto)
            => async operation =>
            {
                var user = await UserManager.FindByNameAsync(signinDto.Username);

                if (user == null || user.DateDeleted.HasValue || user.Type != signinDto.Type)
                    return operation.SetFailed("login failed", OperationResultTypes.Forbidden);

                if (user.DateBlocked.HasValue)
                    return operation.SetFailed("blocked", OperationResultTypes.Forbidden);

                var loginResult = await SignInManager.CheckPasswordSignInAsync(user, signinDto.Password, false);

                if (loginResult == SignInResult.Success)
                {
                    user.DeviceToken = signinDto.DeviceToken;
                    var updateResult = await UserManager.UpdateAsync(user);
                    if (updateResult.Succeeded)
                    {
                        AccountDto accountDto = await FillAccount(user, true);
                        accountDto.HasSubject = await Context.SubjectFacultyAppUsers.AnyAsync(su => su.AppUserId == accountDto.Id);
                        AssignRefreshTokenIfRememberMe(signinDto.RememberMe, accountDto);

                        await TemporaryFillingInBlacklist(user);

                        await TemporaryGenerationStamp(user);

                        return operation.SetSuccess(accountDto);
                    }
                }
                return operation.SetFailed("login failed", OperationResultTypes.Forbidden);
            };
        private Func<OperationResult<AccountDto>, Task<OperationResult<AccountDto>>> _refreshToken(string id, string refreshToken, string generationStamp)
            => async operation =>
            {
                var user = await UserManager.FindByIdAsync(id);

                if (user is null || user.DateDeleted.HasValue)
                    return operation.SetFailed("no user found", OperationResultTypes.Forbidden);

                if (user.DateBlocked.HasValue)
                    return operation.SetFailed("blocked", OperationResultTypes.Forbidden);

                if (!user.PasswordHash.Equals(refreshToken))
                    return operation.SetFailed("refreshToken expired", OperationResultTypes.Forbidden);

                bool isInblacklist = await CheckNoFillingInBlacklist(user);
                if (isInblacklist)
                    return operation.SetFailed("user unsigned", OperationResultTypes.Forbidden);

                if (user.Type != UserTypes.Seller)
                    if (user.GenerationStamp != generationStamp)
                        return operation.SetFailed("generation stamp", OperationResultTypes.Unauthorized);

                AccountDto accountDto = await FillAccount(user);

                return operation.SetSuccess(accountDto);
            };
        private Func<OperationResult<object>, Task<OperationResult<object>>> _signup(SignupDto signupDto)
            => async operation =>
            {
                AppUser user = new AppUser()
                {
                    Name = signupDto.Name,
                    UserName = signupDto.PhoneNumber,
                    Email = signupDto.Email.IsNullOrEmpty().NestedIF(signupDto.PhoneNumber.ToEmail(), signupDto.Email.EnsureEmail()),
                    PhoneNumber = signupDto.PhoneNumber,
                    Gender = signupDto.Gender,
                    Type = UserTypes.User,
                };

                var identityResult = await UserManager.CreateAsync(user, signupDto.Password);

                if (!identityResult.Succeeded)
                    return operation.SetFailed(String.Join(",", identityResult.Errors.Select(error => error.Description)));

                var roleIdentityResult = await UserManager.AddToRoleAsync(user, SmartStartRoles.User.ToString());

                if (!roleIdentityResult.Succeeded)
                    return operation.SetFailed(String.Join(",", roleIdentityResult.Errors.Select(error => error.Description)));

                return operation.SetSuccess("success singup");
            };
        private Func<OperationResult<IEnumerable<AppUserDto>>, Task<OperationResult<IEnumerable<AppUserDto>>>> _getAllDashboard()
            => async operation => {
                var data = await _query<AppUser>().Where(user => !user.DateDeleted.HasValue && user.Type == UserTypes.Dashboard).
                SelectMany(user => Context.UserRoles.Where(userRoleMapEntry => user.Id == userRoleMapEntry.UserId).DefaultIfEmpty(),
                (user, roleMapEntry) => new { User = (AppUserDto)user, UserRole = roleMapEntry })
                .SelectMany(
                x => Context.Roles.Where(role => role.Id == x.UserRole.RoleId).DefaultIfEmpty(),
                (x, role) => new { User = x.User, Role = role.Name })
                .ToListAsync();

                foreach (var one in data)
                {
                    if (one.Role.IsNullOrEmpty()) continue;
                    one.User.Role = one.Role.ToEnum<SmartStartRoles>();
                }

                return operation.SetSuccess(data.Select(x => x.User));
            };
        private Func<OperationResult<AppUserDto>, Task<OperationResult<AppUserDto>>> _create(AppUserDto account)
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
                    SubscriptionDate = account.SubscriptionDate,
                    Type = UserTypes.Dashboard,
                    DateActivated = account.DateActivated
                };

                var identityResult = await UserManager.CreateAsync(user, account.Password);

                if (!identityResult.Succeeded)
                    return operation.SetFailed(String.Join(",", identityResult.Errors.Select(error => error.Description)));

                var roleIdentityResult = await UserManager.AddToRoleAsync(user, account.Role.ToString());

                if (!roleIdentityResult.Succeeded)
                    return operation.SetFailed(String.Join(",", roleIdentityResult.Errors.Select(error => error.Description)));

                account.Id = user.Id;

                return operation.SetSuccess(account);
            };
        private Func<OperationResult<AppUserDto>, Task<OperationResult<AppUserDto>>> _update(AppUserDto account)
            => async operation =>
            {
                AppUser User = await UserManager.FindByIdAsync(account.Id.ToString());

                if (User is null || User.Type != UserTypes.Dashboard)
                    return OperationResultTypes.NotExist;

                User.UserName = account.UserName;
                User.Email = account.Email;
                User.Name = account.Name;
                User.PhoneNumber = account.PhoneNumber;
                User.Birthday = account.Birthday;
                User.Address = account.Address;
                User.Gender = account.Gender;
                User.DateBlocked = account.DateBlocked;
                User.SubscriptionDate = account.SubscriptionDate;
                User.Type = UserTypes.Dashboard;

                if (!account.Password.IsNullOrEmpty())
                    User.PasswordHash = UserManager.PasswordHasher.HashPassword(User, account.Password);

                IdentityResult identityResult = await UserManager.UpdateAsync(User);
                if (!identityResult.Succeeded)
                    return operation.SetFailed(String.Join(",", identityResult.Errors.Select(error => error.Description)));

                // update role
                var role = (await UserManager.GetRolesAsync(User)).First();
                if (account.Role.ToString() != role)
                {
                    var removeRoleIdentityResult = await UserManager.RemoveFromRoleAsync(User, role);

                    if (!removeRoleIdentityResult.Succeeded)
                        return operation.SetFailed(String.Join(",", removeRoleIdentityResult.Errors.Select(error => error.Description)));

                    var addRoleIdentityResult = await UserManager.AddToRoleAsync(User, account.Role.ToString());

                    if (!addRoleIdentityResult.Succeeded)
                        return operation.SetFailed(String.Join(",", addRoleIdentityResult.Errors.Select(error => error.Description)));
                }

                account.Password = string.Empty;

                await PushUserInBlacklist(User, true);

                return operation.SetSuccess(account);
            };
        private Func<OperationResult<bool>, Task<OperationResult<bool>>> _delete(Guid id)
            => async operation =>
            {
                AppUser User = await UserManager.FindByIdAsync(id.ToString());
                if (User is null || User.Type != UserTypes.Dashboard)
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
                if (User is null || User.Type != UserTypes.Dashboard)
                    return OperationResultTypes.NotExist;

                User.DateBlocked = User.DateBlocked.HasValue ? null : DateTime.Now.ToLocalTime();
                IdentityResult identityResult = await UserManager.UpdateAsync(User);
                if (!identityResult.Succeeded)
                    return operation.SetFailed(String.Join(",", identityResult.Errors.Select(error => error.Description)));

                await PushUserInBlacklist(User);

                return operation.SetSuccess(true, User.DateBlocked.HasValue ? "blocked" : "unblocked");
            };
        private Func<OperationResult<AppUserDto>, Task<OperationResult<AppUserDto>>> _details(Guid id)
         => async operation => {

             AppUser User = await UserManager.FindByIdAsync(id.ToString());

             if (User is null || User.Type != UserTypes.Dashboard)
                 return OperationResultTypes.NotExist;

             AppUserDto UserDto = (AppUserDto)User;
             UserDto.Role = (SmartStartRoles)Enum.Parse(typeof(SmartStartRoles), (await UserManager.GetRolesAsync(User)).First());

             return operation.SetSuccess(UserDto);
         };
        private Func<OperationResult<AccountDto>, Task<OperationResult<AccountDto>>> _edit(Guid id, SignupDto dto, string oldPassword)
            => async operation =>
            {
                var user = await UserManager.FindByIdAsync(id.ToString());
                if (user == null || user.DateDeleted.HasValue || user.Type != UserTypes.User)
                    return operation.SetFailed("no user found", OperationResultTypes.Forbidden);

                if (user.DateBlocked.HasValue)
                    return operation.SetFailed("blocked", OperationResultTypes.Forbidden);

                var resultUser = await UserManager.ChangePasswordAsync(user, oldPassword, dto.Password);

                if (resultUser.Succeeded)
                {
                    user.Name = dto.Name;
                    user.UserName = dto.PhoneNumber;
                    user.Email = dto.Email.IsNullOrEmpty().NestedIF(dto.PhoneNumber.ToEmail(), dto.Email.EnsureEmail());
                    user.Gender = dto.Gender;
                    user.PhoneNumber = dto.PhoneNumber;

                    var resultUpdate = await UserManager.UpdateAsync(user);
                    if (resultUpdate == IdentityResult.Success)
                    {
                        AccountDto accountDto = await FillAccount(user);

                        await PushUserInBlacklist(user);
                        await TemporaryFillingInBlacklist(user);

                        return operation.SetSuccess(accountDto);
                    }
                    return operation.SetFailed("edit failed", OperationResultTypes.Forbidden);
                }
                return operation.SetFailed("edit failed", OperationResultTypes.Forbidden);
            };
        
        #region Helper Function
        private async Task<AccountDto> FillAccount(AppUser user, bool isNewGeneration = false)
        {
            var roles = await UserManager.GetRolesAsync(user);
            var expierDate = DateTime.Now.AddMinutes(GlobalConstValues.DefaultExpireTokenMinut);

            if (isNewGeneration)
            {
                user.GenerationStamp = Guid.NewGuid().ToString();
                await UserManager.UpdateAsync(user);
            }

            var UserDto = new AccountDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Address = user.Address,
                Birthday = user.Birthday,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Token = GenerateJwtToken(user, roles, expierDate),
                RefreshToken = user.PasswordHash,
                SubscriptionDate = user.SubscriptionDate,
                Gender = user.Gender
            };
            return UserDto;
        }
        private string GenerateJwtToken(AppUser user, IList<string> roles, DateTime expierDate)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ElClaimTypes.GenerateDate, DateTime.Now.ToLocalTime().ToString()),
                new Claim(ElClaimTypes.GenerationStamp, user.GenerationStamp ),
                new Claim(ElClaimTypes.TransferredProps, System.Text.Json.JsonSerializer.Serialize(new Dictionary<string,object>() { [GlobalConstValues.TransferredProp_FacultyId]=user.FacultyId }) ),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(Config["Jwt:Issuer"],
                  Config["Jwt:Issuer"],
                  claims,
                  expires: expierDate,
                  signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// used by dashboard and clients
        /// </summary>
        /// <param name="rememberMe"></param>
        /// <param name="accountDto"></param>
        private void AssignRefreshTokenIfRememberMe(bool rememberMe, AccountDto accountDto) => _ = !rememberMe ? accountDto.RefreshToken = string.Empty : string.Empty;

        const string DistributedLey_label = "smart_start_user_blacklist:";
        private async Task TemporaryFillingInBlacklist(AppUser user)
        {
            ///TODO ElKood.Web
            //  stop all last token from this time and active other token from date now until last tokens dead
            var isBlock = await Cache.GetStringAsync(DistributedLey_label + user.Id) != null;
            if (isBlock)
                await Cache.SetStringAsync(DistributedLey_label + user.Id, DateTime.Now.ToLocalTime().ToString()); //update
        }
        private async ValueTask TemporaryGenerationStamp(AppUser user)
        {
            if (user.Type == UserTypes.Seller)
                return;
            await Cache.SetStringAsync("smart_start_user_generation_stamp:" + user.Id, user.GenerationStamp, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(GlobalConstValues.DefaultExpireTokenMinut),
                SlidingExpiration = TimeSpan.FromMinutes(GlobalConstValues.DefaultExpireTokenMinut)
            }); //set
        }
        private async Task<bool> CheckNoFillingInBlacklist(AppUser user)
        {
            ///TODO Elkood.Web
            //ckeck secure  'no date yet'  , that mean force user to re login
            var notDate = await Cache.GetStringAsync(DistributedLey_label + user.Id);
            var isInblacklist = !string.IsNullOrEmpty(notDate) && !DateTime.TryParse(notDate, out _);
            return isInblacklist;
        }
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
        #endregion
    }
}

