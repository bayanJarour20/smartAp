using Elkood.Web.Helper.ExtensionMethods.Boolean;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.SharedKernel.ExtensionMethods
{
    public static class ExtensionMethodsShared
    {
        public static object GetDictionaryDescriptions<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                       .Cast<T>()
                       .Select(t => new { Key = t.ToString(), Id = (int)(object)t, Name = GetDisplayDescription(t) })
                       .Where(x => x.Name != string.Empty);
        }
        public static string GetDisplayDescription(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                           ?.FirstOrDefault()?
                            .GetCustomAttribute<DisplayAttribute>()?
                            .GetDescription() ?? string.Empty;
        }
        public static string ToEmail(this string value, string inc = "SmartStart", string org = "me") => $"{value}@{inc}.{org}";
        public static string EnsureEmail(this string value) => new EmailAddressAttribute().IsValid(value).NestedIF(value, value.ToEmail());
    }
}
