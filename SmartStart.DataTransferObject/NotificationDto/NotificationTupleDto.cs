using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.NotificationDto
{
    public class NotificationTupleDto
    {
        public List<NotificationToMeDto> NotificationToday { get; set; }
        public List<NotificationToMeDto> NotificationAll { get; set; }
    }
}
