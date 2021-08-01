using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Constants;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using SmartStart.Model.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartStart.Model.Business
{
    public class Invoice : BaseEntity<Guid>
    {
        public Invoice()
        {
            Codes = new HashSet<Code>();
        }

        [ColumnDataType(DataBaseTypes.VARCHAR, TypeConstants.ShortString)]
        public string SerialNumber { get; set; }

        [ColumnDataType(DataBaseTypes.DATETIME2)]
        public DateTime Date { get; set; }

        [ColumnDataType(DataBaseTypes.FLOAT, 4)]
        public float Rate { get; set; }

        /// <summary>
        /// Total part-All of Price codes
        /// </summary>
        [ColumnDataType(DataBaseTypes.FLOAT)]
        public double Total { get; set; }

        [ColumnDataType(DataBaseTypes.NVARCHAR)]
        public string Note { get; set; }


        [Required]
        public Guid SellerId { get; set; }
        public AppUser Seller { get; set; }

        public ICollection<Code> Codes { get; set; }
    }
}
