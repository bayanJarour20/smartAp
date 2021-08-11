using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.CodeDto
{
    public class CodeDetailsDto : CodeDto
    {
        public string UserName { get; set; }
        public Guid? UserId { get; set; }
        public string SellerName { get; set; }
        public Guid SellerId { get; set; }
        public double PaidValue { get; set; }
        public string PackageName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
