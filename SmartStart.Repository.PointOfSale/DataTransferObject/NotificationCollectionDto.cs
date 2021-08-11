using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.PointOfSale.PointOfSaleService.DataTransferObject
{
    public class NotificationCollectionDto
    {
        public IEnumerable<NotificationDto> NotificationToday { get; set; }
        public IEnumerable<NotificationDto> Notification { get; set; }
    }
}
