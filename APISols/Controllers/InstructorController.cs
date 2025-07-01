using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entities.DTO;
using Entities.Models;

namespace APISols.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly AppDbContext _context;
        public InstructorController(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var allInstructor = _context.Instructors
                                .Select(i => new InstructorDto { Id = i.Id, Name = i.Name })
                                .ToList();
            return Ok(allInstructor);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var instructor = _context.Instructors.FirstOrDefault(i => i.Id == id);
            if (instructor == null) 
            {
                return BadRequest("Instructor not found.");
            }
            var DTO = new InstructorDto { Id = instructor.Id,Name = instructor.Name };
            return Ok(DTO);
        }

        [HttpPost]
        public IActionResult Add([FromBody] InstructorDto instructorDto)
        {
            var instructor = new Instructor() { Name = instructorDto.Name};
            _context.Instructors.Add(instructor);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get),new{ id = instructorDto.Id },instructorDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] InstructorDto instructorDto)
        {
            var instructor = _context.Instructors.FirstOrDefault(x=>x.Id == id);

            if (instructor == null) 
            {
                return BadRequest("Instructor not found.");
            }
            instructor.Name = instructorDto.Name;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var instructor = _context.Instructors.FirstOrDefault(x => x.Id == id);

            if (instructor == null)
            {
                return BadRequest("Instructor not found.");
            }
            _context.Instructors.Remove(instructor);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
