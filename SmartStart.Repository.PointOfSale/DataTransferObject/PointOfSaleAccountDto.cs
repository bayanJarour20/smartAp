using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.PointOfSale.PointOfSaleService.DataTransferObject
{
    public class PointOfSaleAccountDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? DateBlocked  { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        public double? MoneyLimit { get; set; }
        public DateTime? SubscriptionDate { get; set; }
        public int CodeSoldCount { get; set; }
        public bool AllowDiscount { get; set; }
        public Guid CityId { get; set; }
        public DateTime? DateActivated { get; set; }
        public double Rate { get; set; }
        public IEnumerable<Guid> facList { get; set; }
    }
}
