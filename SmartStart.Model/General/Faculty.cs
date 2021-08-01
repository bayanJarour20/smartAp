using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Constants;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using Elkood.Web.Infrastructure.ModelEntity.Interface;
using SmartStart.Model.Main;
using SmartStart.Model.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartStart.Model.General
{
    public class Faculty : BaseEntity<Guid>, INominal
    {
        public Faculty()
        {
            Subjects = new HashSet<Subject>();
            AppUsers = new HashSet<AppUser>();
        }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.PhraseString)]
        [Required]
        public string Name { get; set; }

        [ColumnDataType(DataBaseTypes.TINYINT)]
        [DefaultValue(1)]
        public int NumberOfYear { get; set; }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.MediumString)]
        public string ImagePath { get; set; }

        [Required]
        public Guid UniversityId { get; set; }
        public University University { get; set; }

        public ICollection<Subject> Subjects { get; set; }
        public ICollection<AppUser> AppUsers { get; set; }
    }
}
