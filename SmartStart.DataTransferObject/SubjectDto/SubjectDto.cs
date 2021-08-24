using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.SubjectDto
{
    public class SubjectDto : SubjectBaseDto
    {
        public int ExamCount { get; set; }
        public int BankCount { get; set; }
        public int MicroscopeCount { get; set; }
        public int InterviewCount { get; set; }
        public DateTime DateCreate { get; set; }
    }
   
}
