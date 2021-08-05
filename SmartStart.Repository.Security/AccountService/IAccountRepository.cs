using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using SmartStart.DataTransferObject.AccountDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Security.AccountService
{
    public interface IAccountRepository : IElRepository
    {
        Task<OperationResult<AccountDto>> Login(LoginDto loginDto);
        Task<OperationResult<AccountDto>> Signin(SigninDto loginDto);
        Task<OperationResult<AccountDto>> RefreshToken(string id, string refreshToken, string generationStamp);
        Task<OperationResult<object>> Signup(SignupDto signupDto);
        Task<OperationResult<IEnumerable<AppUserDto>>> GetAllDashboard();
        Task<OperationResult<AppUserDto>> Create(AppUserDto account);
        Task<OperationResult<AppUserDto>> Update(AppUserDto account);
        Task<OperationResult<bool>> Delete(Guid id);
        Task<OperationResult<bool>> Block(Guid id);
        Task<OperationResult<AppUserDto>> Details(Guid id);
        Task<OperationResult<AccountDto>> Edit(Guid id, SignupDto dto, string oldPassword);
    }
}
