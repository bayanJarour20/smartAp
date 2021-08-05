using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.NotificationDto
{
    public class NotificationToMeDto : NotificationDto
    {
        public bool IsToMe { get; set; }
    }
}
