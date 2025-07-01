using DataAccess.Data;
using Entities.DTO;
using Entities.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APISols.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {

        private readonly AppDbContext _context;
        public CourseController(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var courses = _context.Courses
                                   .Select(c => new CourseDto {Id = c.Id,Name=c.Name,InstId=c.InstID})
                                   .ToList();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null) 
            {
                return BadRequest("Course Not Found");
            }
            var dto = new CourseDto()
            {
                Id = course.Id,
                Name = course.Name,
                InstId = course.InstID,
            };
            
            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Add([FromBody] CourseDto courseDto)
        {
            var course = new Course {Id=courseDto.Id, Name=courseDto.Name, InstID=courseDto.InstId, };
            _context.Courses.Add(course);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new {id=courseDto.Id }, courseDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CourseDto courseDto)
        {
            var course = _context.Courses.FirstOrDefault(c=>c.Id == id);
            if (course == null)
            {
                return BadRequest("Course Not Found");
            }
            course.Name = courseDto.Name;
            course.InstID = courseDto.InstId;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var course = _context.Courses.FirstOrDefault(c=>c.Id == id);
            if (course == null)
            {
                return BadRequest("Course Not Found");
            }
            _context.Courses.Remove(course);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
