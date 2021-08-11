using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.Repository.PointOfSale.PointOfSaleService.DataTransferObject
{
    public class NotificationDto
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public bool HasReaded { get; set; }
    }

}
