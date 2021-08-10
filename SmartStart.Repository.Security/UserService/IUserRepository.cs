using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext;
using SmartStart.DataTransferObject.AccountDto;
using SmartStart.DataTransferObject.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Security.UserService
{
    public interface IUserRepository : IElRepository
    {
        Task<OperationResult<IEnumerable<AppUserDto>>> GetAllUser();
        Task<OperationResult<AppUserDto>> Create(AppUserDto account);
        Task<OperationResult<AppUserDto>> Update(AppUserDto account);
        Task<OperationResult<bool>> Delete(Guid id);
        Task<OperationResult<bool>> Block(Guid id);
        Task<OperationResult<UserDetailsDto>> Details(Guid id);

        Task<OperationResult<bool>> DeleteRange(IEnumerable<Guid> ids);
    }
}
