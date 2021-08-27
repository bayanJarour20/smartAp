using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartStart.SharedKernel.Security
{
    public enum SmartStartRoles
    {
        [Display(Description = "مسؤول")]
        Admin = 1,
        User = 2,
        Seller = 3,
        [Display(Description = "مدخل بيانات")]
        Entry = 4,
    }
}
