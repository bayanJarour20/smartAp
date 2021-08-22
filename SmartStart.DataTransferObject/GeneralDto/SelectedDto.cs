using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.GeneralDto
{
    public class SelectedDto
    {
        public Guid FacultyId { get; set; }
        public Guid SectionId { get; set; }
        public int Year { get; set; }
        public Guid SemesterId { get; set; }
    }
}
