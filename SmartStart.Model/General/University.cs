using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Constants;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using Elkood.Web.Infrastructure.ModelEntity.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartStart.Model.General
{
    public class University : BaseEntity<Guid>, INominal
    {
        public University()
        {
            Faculties = new HashSet<Faculty>();
        }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.PhraseString)]
        [Required]
        public string Name { get; set; }

        [Required]
        public Guid CityId { get; set; }
        public City City { get; set; }

        public ICollection<Faculty> Faculties { get; set; }
    }
}
