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
    public class Code : BaseEntity<Guid>
    {
        public Code()
        {
            CodePackages = new HashSet<CodePackage>();
        }

        [ColumnDataType(DataBaseTypes.VARCHAR, TypeConstants.ShortString)]
        public string Hash { get; set; }

        /// <summary>
        /// Total Package-prices
        /// </summary>
        [ColumnDataType(DataBaseTypes.FLOAT)]
        public double Value { get; set; }

        [ColumnDataType(DataBaseTypes.DATETIME2)]
        public DateTime? DateActivated { get; set; }

        [ColumnDataType(DataBaseTypes.FLOAT)]
        public double DiscountRate { get; set; }

        /// <summary>
        /// Maximum package end
        /// </summary>
        [ColumnDataType(DataBaseTypes.DATETIME2)]
        public DateTime MaxEndDate { get; set; }


        [Required]
        public Guid SellerId { get; set; }
        public AppUser Seller { get; set; }

        public Guid? UserId { get; set; }
        public AppUser User { get; set; }

        public Guid? InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public ICollection<CodePackage> CodePackages { get; set; }
    }
}
