using Elkood.Web.Helper.Validations.Attribute.DataBaseAnnotations;
using Elkood.Web.Helper.Validations.Constants;
using Elkood.Web.Helper.Validations.Enum;
using Elkood.Web.Infrastructure.ModelEntity.Interface;
using Elkood.Web.Infrastructure.ModelEntity.Securty;
using SmartStart.Model.Business;
using SmartStart.Model.General;
using SmartStart.Model.Main;
using SmartStart.Model.Setting;
using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartStart.Model.Security
{
    public class AppUser : ElIdentityUser<Guid>, INominal
    {
        public AppUser()
        {
            UserNotifications = new HashSet<UserNotification>();
            Invoices = new HashSet<Invoice>();
            Rates = new HashSet<Rate>();
            SubjectAppUsers = new HashSet<SubjectFacultyAppUser>();
            Codes = new HashSet<Code>();
            UserCodes = new HashSet<Code>();
            Feedbacks = new HashSet<Feedback>();
            Faculties = new HashSet<FacultyPOSUser>();
        }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.MediumString)]
        [Required]
        public string Name { get; set; }

        [ColumnDataType(DataBaseTypes.DATE)]
        public DateTime? Birthday { get; set; }

        [ColumnDataType(DataBaseTypes.DATETIME2)]
        public DateTime? DateBlocked { get; set; }

        [ColumnDataType(DataBaseTypes.TINYINT)]
        public UserTypes Type { get; set; }

        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.MediumString)]
        public string Address { get; set; }

        [ColumnDataType(DataBaseTypes.BIT)]
        public bool Gender { get; set; }

        [ColumnDataType(DataBaseTypes.FLOAT)]
        public double? MoneyLimit { get; set; }

        [ColumnDataType(DataBaseTypes.VARCHAR, TypeConstants.LongString)]
        public string DeviceToken { get; set; }

        [ColumnDataType(DataBaseTypes.DATETIME2)]
        public DateTime? SubscriptionDate { get; set; }

        [ColumnDataType(DataBaseTypes.BIT)]
        public bool AllowDiscount { get; set; }

        [ColumnDataType(DataBaseTypes.DATETIME2)]
        public DateTime? DateActivated { get; set; }


        public string GenerationStamp { get; set; }


        public Guid? FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public Guid? CityId { get; set; }
        public City City { get; set; }

        public ICollection<UserNotification> UserNotifications { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<Rate> Rates { get; set; }
        public ICollection<SubjectFacultyAppUser> SubjectAppUsers { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<FacultyPOSUser> Faculties { get; set; }


        [InverseProperty(nameof(Code.Seller))]
        public ICollection<Code> Codes { get; set; }

        [InverseProperty(nameof(Code.User))]
        public ICollection<Code> UserCodes { get; set; }
    }
}
