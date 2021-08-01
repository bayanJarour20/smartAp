using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using SmartStart.Model.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartStart.Model.Business
{
    public class Rate : BaseEntity<Guid>
    {
        [ColumnDataType(DataBaseTypes.FLOAT)]
        public double DiscountRate { get; set; }

        [Required]
        public Guid SellerId { get; set; }
        public AppUser Seller { get; set; }
    }
}
