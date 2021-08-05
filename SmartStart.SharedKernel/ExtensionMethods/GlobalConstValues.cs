using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStart.SharedKernel.ExtensionMethods
{
    public class GlobalConstValues
    {
        public static Guid DefaultSellerId = new("{AC140878-B8C0-47F6-AAC7-6E6E601A238B}");
        /// <summary>
        /// شاملة لسنة
        /// </summary>
        public static Guid DefaultConstPackageId = new("{18CAB447-6308-4EA5-9303-6CD565090FE8}");
        /// <summary>
        /// شاملة لسنة مع مواد حمل 4
        /// </summary>
        public static Guid DefaultConstOptionalPackageId = new("{6427905A-E838-4C03-8DAE-BA3EB7B9CF4B}");
        /// <summary>
        /// مواد فقط 4
        /// </summary>
        public static Guid DefaultOptionalPackageId = new("{E3771D3C-528C-4501-9AAF-806E349A92BF}");
        /// <summary>
        /// كود بيفتح كلشي
        /// </summary>
        public static Guid DefaultSuperCodeId = new("{E3271D3C-528C-4501-9AAF-806E349A92BF}");
        /// <summary>
        /// حزمة مفتوحة 
        /// </summary>
        public static Guid SuperPackageId = new("{18CAB441-6308-4EA5-9303-6CD565090FE8}");

        public const int DefaultExpireTokenMinut = 1 * 24 * 60;

        public const string TransferredProp_FacultyId = "facultyId";
    }
}
