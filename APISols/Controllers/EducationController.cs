using DataAccess.Data;
using Entities.DTO;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APISols.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public EducationController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetCourseWithInstructor()
        {
            var instructors = _dbContext.Instructors.Include(i => i.Courses).ToList();
            var result = new List<CourseWithInstructorDto>();

            foreach (var instructor in instructors)
            {
                foreach (var course in instructor.Courses)
                {
                    var dto = new CourseWithInstructorDto
                    {
                        CourseName = course.Name,
                        InstructorName = instructor.Name
                    };

                    result.Add(dto);
                }
            }

            return Ok(result);
        }


        [HttpGet("InstructorWithCourses")]
        public IActionResult GetInstructorWithCourses()
        {
            var instructors = _dbContext.Instructors.Include(i => i.Courses).ToList();
            var result = new List<InstructorWithCoursesDto>();

            foreach (var instructor in instructors)
            {
                var dto = new InstructorWithCoursesDto();
                dto.InstructorName = instructor.Name;
                dto.CoursesName = new List<string>();

                foreach (var course in instructor.Courses)
                {
                    dto.CoursesName.Add(course.Name);
                }

                result.Add(dto);
            }

            return Ok(result);
        }

    }
}
