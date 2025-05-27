using Lms_Backend.Interfaces;
using Lms_Backend.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Lms_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        /// <summary>
        /// Retrieves all courses.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_courseService.GetAllCourses());
        }

        /// <summary>
        /// Retrieves all courses along with their enrolled students.
        /// </summary>
        /// <returns></returns>
        [HttpGet("with-students")]
        public ActionResult<List<CourseWithStudentsDto>> GetCoursesWithStudents()
        {
            return Ok(_courseService.GetAllCoursesWithStudents());
        }

        /// <summary>
        /// Retrieves a course by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("ID is required.");

            var course = _courseService.GetCourseById(id);
            if (course == null) return NotFound();
            return Ok(course);
        }

        /// <summary>
        /// Creates a new course - couse id is generated automatically.
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] Course course)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _courseService.AddCourse(course);
                return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
            }
            catch (Exception ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing course by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Course course)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("ID is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = _courseService.UpdateCourse(id, course);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a course by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("ID is required.");

            var result = _courseService.DeleteCourse(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}