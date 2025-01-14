﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.DataTransferObject.FacultyDto
{
    public class FacultyBaseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UniversityId { get; set; }
        public int NumOfYears { get; set; }
        public int SubjectCount { get; set; }
    }
}
