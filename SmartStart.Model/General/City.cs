using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Constants;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using Elkood.Web.Infrastructure.ModelEntity.Interface;
using SmartStart.Model.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartStart.Model.General
{
    public class City : BaseEntity<Guid>, INominal
    {
        public City()
        {
            Universities = new HashSet<University>();
            AppUsers = new HashSet<AppUser>();
        }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.PhraseString)]
        [Required]
        public string Name { get; set; }

        public ICollection<University> Universities { get; set; }
        public ICollection<AppUser> AppUsers { get; set; }
    }
}
