using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Constants;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Base;
using Elkood.Web.Infrastructure.ModelEntity.Interface;
using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStart.Model.Business
{
    public class Package : BaseEntity<Guid>, INominal
    {
        public Package()
        {
            CodePackages = new HashSet<CodePackage>();
            PackageSubjects = new HashSet<PackageSubject>();
        }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.MediumString)]
        public string Name { get; set; }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.LongString)]
        public string Description { get; set; }

        /// <summary>
        /// Total PackageExam-Prices
        /// </summary>
        [ColumnDataType(DataBaseTypes.FLOAT)]
        public double Price { get; set; }

        [ColumnDataType(DataBaseTypes.TINYINT)]
        public PackageTypes Type { get; set; }

        [ColumnDataType(DataBaseTypes.DATETIME2)]
        public DateTime StartDate { get; set; }

        [ColumnDataType(DataBaseTypes.DATETIME2)]
        public DateTime EndDate { get; set; }

        [ColumnDataType(DataBaseTypes.BIT)]
        public bool IsHidden { get; set; }


        public ICollection<CodePackage> CodePackages { get; set; }
        public ICollection<PackageSubject> PackageSubjects { get; set; }
    }
}
