using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.FacultyDto
{
    public class UserFacultyList
    {
        public Guid Id { get; set; }

        public List<FacultySelectDto> Faculties { get; set; }

    }
}
