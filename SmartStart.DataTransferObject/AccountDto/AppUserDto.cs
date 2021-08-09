using SmartStart.Model.Security;
using SmartStart.SharedKernel.Enums;
using SmartStart.SharedKernel.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.AccountDto
{
    public class AppUserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? DateBlocked { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateActivated { get; set; }
        public DateTime? SubscriptionDate { get; set; }
        public bool Gender { get; set; }
        public UserTypes Type { get; set; }
        public Guid? FacultyId { get; set; }
        public int SubscriptionCount { get; set; }
        public SmartStartRoles? Role { get; set; }
        //public List<Guid> FacultiesIds { get; set; }
        public static explicit operator AppUserDto(AppUser user)
        {
            return new AppUserDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Birthday = user.Birthday,
                DateBlocked = user.DateBlocked,
                DateActivated = user.DateActivated,
                Gender = user.Gender,
                DateCreated = user.DateCreated,
                Role = SmartStartRoles.User,
                SubscriptionDate = user.SubscriptionDate,
                SubscriptionCount = user.UserCodes.Count,
                Type = user.Type,
                FacultyId = user.FacultyId
            };
        }
    }
}
