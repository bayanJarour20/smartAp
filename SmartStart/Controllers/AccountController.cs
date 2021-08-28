using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Helper.ExtensionMethods.String;
using Elkood.Web.MVC;
using Elkood.Web.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartStart.DataTransferObject.AccountDto;
using SmartStart.Repository.Security.AccountService;
using SmartStart.Security;
using SmartStart.SharedKernel.ExtensionMethods;
using SmartStart.SharedKernel.Security;
using SmartStart.ViewModels.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ElControllerBase<Guid, IAccountRepository>
    {
        public AccountController(IAccountRepository accountRepository) : base(accountRepository) { }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login) => await repository.Login(login).ToJsonResultAsync();

        [HttpPost]
        public async Task<IActionResult> Signin(SigninDto signin) => await repository.Signin(signin).ToJsonResultAsync();

        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromHeader] string user_id, [FromHeader] string refresh_token, [FromHeader] string Authorization)
        {

            if (user_id.IsNullOrEmpty() || refresh_token.IsNullOrEmpty() || Authorization.IsNullOrEmpty())
                return new JsonResult($"user_id: {user_id} , refresh_token: {refresh_token} , Authorization: {Authorization} , Headers " +
                    $"{Request.Headers.Aggregate("", (all, next) => all += " , " + next.Key + ": " + next.Value)}")
                { StatusCode = 403 };

            var generationStamp = DecodeToken?.Claims?.FirstOrDefault(x => x.Type == ElClaimTypes.GenerationStamp);

            var operationResult = await repository.RefreshToken(user_id, refresh_token, generationStamp?.Value ?? string.Empty);
            return operationResult.ToJsonResult();
        }
        ///TODO fixed in Elkood.web nearest Released
        /// JwtSecurityToken?  off #nullable disable , or un decode token on return 401 pre-action entered 
        /// , or is Model Valid with ElMessage  struct see: https://github.com/dotnet/aspnetcore/blob/37764a7f65d5bfb0b0bcdb938f3d218b10986c53/src/Mvc/Mvc.Abstractions/src/ModelBinding/ModelStateDictionary.cs#L147 
        /// http://git.elkood.com/MHozaifaA/Elkood.Web/src/branch/master/Elkood.Web/MVC/Controller/ElControllerBase.cs#L19
        protected override JwtSecurityToken DecodeToken => Token is null ? (this.Request.Headers.FirstOrDefault(x => x.Key == ElClaimTypes.Token) switch
            { { Key: var auth, Value: var value } when auth is not null && value.Count > 0 => new(value[0].Split(" ")[1]), _ => null }) : base.DecodeToken;
        [HttpPost]
        public async Task<IActionResult> Signup(SignupDto signup) => (await repository.Signup(signup)).Into(operation => operation).ToJsonResult();

        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> GetAllDashboard() => await repository.GetAllDashboard().ToJsonResultAsync();
       
        [HttpPost, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> Create(AppUserDto account) => await repository.Create(account).ToJsonResultAsync();

        [HttpPut, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> Update(AppUserDto account) => await repository.Update(account).ToJsonResultAsync();

        [HttpDelete, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> Delete([Required] Guid id) => await repository.Delete(id).IntoAsync(o => o).ToJsonResultAsync();

        [HttpPut("{id}"), ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> Block(Guid id) => await repository.Block(id).IntoAsync(o => o).ToJsonResultAsync();
        
        [HttpGet("{id}"), ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public async Task<IActionResult> Details(Guid id) => await repository.Details(id).ToJsonResultAsync();
       
        [HttpPut, ElAuthorizeDistributed(SmartStartRoles.User)]
        public async Task<IActionResult> Edit(EditAccountViewModel editAccount) => await repository.Edit(Key.Value, editAccount, editAccount.OldPassword).ToJsonResultAsync();

        [HttpGet, ElAuthorizeDistributed(SmartStartRoles.Admin)]
        public IActionResult Roles() => ExtensionMethodsShared.GetDictionaryDescriptions<SmartStartRoles>().ToOperationResult().ToJsonResult();
    }
}
