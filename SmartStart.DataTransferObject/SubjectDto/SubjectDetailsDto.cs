using SmartStart.DataTransferObject.SharedDto;
using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.SubjectDto
{
    public class SubjectDetailsDto : SubjectBaseDto
    {
        //public List<SelectDto> Faculties { get; set; }
        //public SelectDto Semesters { get; set; }
        public List<Guid> Doctors { get; set; }
        public SubjectTypes Type { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool IsFree { get; set; }
    }
}
