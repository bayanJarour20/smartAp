using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.UserDto
{
    public class UserCodeDto
    {
        public Guid Id { get; set; }
        public string Hash { get; set; }
        public string SellerName { get; set; }
        public double Value { get; set; }
        public DateTime? DateActivated { get; set; }
        public double DiscountRate { get; set; }
        public DateTime MaxEndDate { get; set; }

        public UserPackageDto Package { get; set; }
    }
}
