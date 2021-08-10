using Elkood.Web.Common.AuxiliaryImplemental.Interfaces;
using Elkood.Web.Infrastructure.ModelEntity.Interface;
using SmartStart.Model.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.SettingDto
{
    public class SettingDto : ISelector<Setting, SettingDto>, IIndex<Guid>
    {
        public Guid Id { get; set; }
        public string AppVersion { get; set; }
        public string SupportedAppVersions { get; set; }

        public static Expression<Func<Setting, SettingDto>> Selector { get; set; } = setting => new SettingDto() { Id = setting.Id, AppVersion = setting.AppVersion, SupportedAppVersions = setting.SupportedAppVersions };
        public static Expression<Func<SettingDto, Setting>> InverseSelector { get; set; } = setting => new Setting() { Id = setting.Id, AppVersion = setting.AppVersion, SupportedAppVersions = setting.SupportedAppVersions };
        public static Action<SettingDto, Setting> AssignSelector { get; set; } = (dto, entity) => { entity.AppVersion = dto.AppVersion; entity.SupportedAppVersions = dto.SupportedAppVersions; };
    }
}
