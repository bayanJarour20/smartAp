using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.ViewModels.Code
{
    public class ActivateCodeViewModel
    {
        public string Hash { get; set; }
        public IEnumerable<Guid> subjectFacultyIds { get; set; }
    }
}
