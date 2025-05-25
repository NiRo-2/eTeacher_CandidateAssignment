using Lms_Backend.Interfaces;
using Lms_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Lms_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Retrieves all students in the system.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_studentService.GetAllStudents());
        }

        /// <summary>
        /// Retrieves a student by their ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("ID is required.");

            var student = _studentService.GetStudentById(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        /// <summary>
        /// Retrieves a student by their email address.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("by-email/{email}")]
        public IActionResult GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return BadRequest("Email is required.");
            if (!new EmailAddressAttribute().IsValid(email))
                return BadRequest("Invalid email format.");

            var student = _studentService.GetStudentByEmail(email);
            if (student == null) return NotFound();
            return Ok(student);
        }

        /// <summary>
        /// Creates a new student - student id is generated automatically.
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] Student student)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _studentService.AddStudent(student);
                return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing student by their ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Student student)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("ID is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = _studentService.UpdateStudent(id, student);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a student by their ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("ID is required.");

            var result = _studentService.DeleteStudent(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}