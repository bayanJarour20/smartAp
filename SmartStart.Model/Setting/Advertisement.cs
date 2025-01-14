﻿using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Constants;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStart.Model.Setting
{
    public class Advertisement : BaseEntity<Guid>
    {
        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.NounString)]
        public string Title { get; set; }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.MediumString)]
        public string ImagePath { get; set; }

        [ColumnDataType(DataBaseTypes.DATETIME2)]
        public DateTime StartDate { get; set; }

        [ColumnDataType(DataBaseTypes.DATETIME2)]
        public DateTime EndDate { get; set; }

        [ColumnDataType(DataBaseTypes.FLOAT)]
        public double? Price { get; set; }

        [ColumnDataType(DataBaseTypes.TINYINT)]
        public AdvertisementTypes Type { get; set; }

        public string? Description { get; set; }
    }
}
