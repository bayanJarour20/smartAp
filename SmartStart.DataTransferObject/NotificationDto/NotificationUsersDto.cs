using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.NotificationDto
{
    public class NotificationUsersDto : NotificationDto
    {
        public IEnumerable<Guid> UserIds { get; set; }
    }
}
