using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartStart.SharedKernel.Security
{
    public enum SmartStartRoles
    {
        [Display(Description = "مسؤول")]
        Admin = 0,
        User = 1,
        Seller = 2,
        [Display(Description = "مدخل بيانات")]
        Entry = 3,
    }
}
