using Elkood.Web.Common.ContextResult.OperationContext;
using Elkood.Web.Service.BoundedContext.General;
using Elkood.Web.Service.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartStart.DataTransferObject.SettingDto;
using SmartStart.SharedKernel.Enums;
using SmartStart.SqlServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.Setting.SettingService
{
    [ElRepository]
    public class SettingRepository : ElRepositoryGeneral<SmartStartDbContext, Guid, Model.Setting.Setting, SettingDto>, ISettingRepository
    {
        public SettingRepository(SmartStartDbContext context) : base(context) { }

        public async Task<OperationResult<AppVersionStates>> CheckAppVersion(string CurrentAppVersion)
            => await RepositoryHandler(_checkAppVersion(CurrentAppVersion));

        private Func<OperationResult<AppVersionStates>, Task<OperationResult<AppVersionStates>>> _checkAppVersion(string currentAppVersion)
            => async operation =>
            {
                try
                {
                    var CheckSupportedAppVersion = await Context.Settings.AnyAsync(setting => setting.SupportedAppVersions.Contains(currentAppVersion));
                    if(CheckSupportedAppVersion)
                    {
                        var CheckAppVersion = await Context.Settings.AnyAsync(setting => setting.AppVersion.Contains(currentAppVersion));
                        if(CheckAppVersion)
                        {
                            return operation.SetSuccess(AppVersionStates.SupportedState, "Equals AppVersion");
                        }
                        return operation.SetSuccess(AppVersionStates.UpdateAvailableState, "Equals SupportedAppVersion");
                    }
                    return operation.SetSuccess(AppVersionStates.UnSupportedState, "Unequals SupportedAppVersion");
                }
                catch (Exception ex)
                {
                    return operation.SetSuccess(AppVersionStates.UnSupportedState, "Setting row null");
                }
            };
        
    }
}
