using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.UserDto
{
    public class UserFacultyDto
    {
        public Guid Id { get; set; }
        public List<Guid> Faculties { get; set; }
    }
}
