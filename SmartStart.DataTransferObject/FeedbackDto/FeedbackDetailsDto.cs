using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.FeedbackDto
{
    public class FeedbackDetailsDto : FeedbackDto
    {
        public string AppHserName { get; set; }
        public DateTime SendDate { get; set; }
    }
}
