using SmartStart.DataTransferObject.AccountDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.UserDto
{
    public class UserDetailsDto : AppUserDto
    {
        public IEnumerable<UserCodeDto> Codes { get; set; }
    }
}
