using SmartStart.DataTransferObject.SharedDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.SubjectDto
{
    public class SubjectDetailsDto : SubjectBaseDto
    {
        public List<SelectDto> Faculties { get; set; }
        public List<SelectDto> Semesters { get; set; }
        public List<SelectDto> Doctors { get; set; }
        public SelectDto MyProperty { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
