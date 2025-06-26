using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class InstructorWithCoursesDto
    {
        public string InstructorName { get; set; }
        public List<string> CoursesName { get; set; }
    }
}
