using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.AccountDto
{
    public class SigninDto
    {
        /// <summary>
        /// Username , Email or PhoneNumber
        /// </summary>
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; } = true;
        public string DeviceToken { get; set; }
        public UserTypes Type { get; set; }

    }
}
