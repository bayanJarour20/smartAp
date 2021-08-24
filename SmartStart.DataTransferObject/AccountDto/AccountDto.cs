using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.AccountDto
{
    public class AccountDto : TokenDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? SubscriptionDate { get; set; }
        public bool HasSubject { get; set; }
        public bool Gender { get; set; }
    }
}
