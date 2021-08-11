using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.SharedKernel.Enums
{
    public enum CodeTypes
    {
        [Display(Description = "مخصصة")]
        Normal = 0,
        /// <summary>
        /// شامل
        /// </summary>
        [Display(Description = "شاملة لسنة")]
        Const = 1,
        /// <summary>
        /// شامل مع حمل
        /// </summary>
        [Display(Description = "شاملة لسنة مع مواد حمل")]
        ConstOptional = 2,
        ///// <summary>
        ///// 4 مواد
        ///// </summary>
        [Display(Description = "محدودة 4 مواد")]
        Optional = 3,
    }
}
