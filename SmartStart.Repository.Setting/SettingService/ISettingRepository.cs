using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext.General;
using SmartStart.DataTransferObject.SettingDto;
using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Setting.SettingService
{
    public interface ISettingRepository : IElRepositoryGeneral<Guid, Model.Setting.Setting ,SettingDto>
    {
        Task<OperationResult<AppVersionStates>> CheckAppVersion(string CurrentAppVersion);
    }
}
