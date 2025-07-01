
using DataAccess.Data;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace APISols.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public StudentController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Create

        [HttpPost("AddStudent")]
        public IActionResult Add(Student student)
        {
            _dbContext.Students.Add(student);
            _dbContext.SaveChanges();
            return Ok(student);
        }
        #endregion

        #region Read
        [HttpGet("GetAll")]
        public IActionResult GetALl()
        {
            return Ok(_dbContext.Students.ToList());
        }

        [HttpGet]
        public IActionResult GetById([FromQuery]int id)
        {
            var s = _dbContext.Students.FirstOrDefault(x => x.Id == id);
            return Ok(s);
        }
        #endregion

        #region Update

        [HttpPut("Update/{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody]Student newStudent)
        {
            var dbStudent = _dbContext.Students.FirstOrDefault(x => x.Id == id);
            if (newStudent != null)
            {
                dbStudent.Name = newStudent.Name;
                dbStudent.GPA = newStudent.GPA;

                _dbContext.SaveChanges();

                return NoContent();
            }
            else { return NotFound(); }

        }
        #endregion

        #region Delete

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.Id == id);
            _dbContext.Students.Remove(student);
            _dbContext.SaveChanges();
            return Ok(student);
        }

        #endregion


       



    }
}
