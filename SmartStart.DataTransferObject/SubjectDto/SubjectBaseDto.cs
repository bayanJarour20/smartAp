using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.SubjectDto
{
    public class SubjectBaseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<SubjectFacultyDto> subjectFaculties { get; set; }
    }
    public class SubjectFacultyDto
    {
        public Guid FacultyId { get; set; }
        public Guid? SemesterId { get; set; }
        public Guid? SectionId { get; set; }
        public Guid? DoctorId { get; set; }
        public int Year { get; set; }
        public double Price { get; set; }
    }
}
