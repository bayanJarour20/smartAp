using SmartStart.DataTransferObject.AccountDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.ViewModels.Security
{
    public class EditAccountViewModel : SignupDto
    {
        public string OldPassword { get; set; }
    }
}
