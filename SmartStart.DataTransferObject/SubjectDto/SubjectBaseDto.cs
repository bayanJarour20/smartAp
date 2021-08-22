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
        public string FacultyName { get; set; }
        public Guid? SemesterId { get; set; }
        public string SemesterName { get; set; }
        public Guid? SectionId { get; set; }
        public string SectionName { get; set; }
        public Guid? DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int Year { get; set; }
        public double Price { get; set; }
    }
}
