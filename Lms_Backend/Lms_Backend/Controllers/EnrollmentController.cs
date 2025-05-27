using Lms_Backend.Interfaces;
using Lms_Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lms_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        /// <summary>
        /// Retrieves all enrollments.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_enrollmentService.GetAllEnrollments());
        }

        /// <summary>
        /// Retrieves all enrollments with details (DTOs).
        /// </summary>
        /// <returns></returns>
        [HttpGet("with-details")]
        public IActionResult GetAllWithDetails()
        {
            var dtos = _enrollmentService.GetAllEnrollmentDtos();
            return Ok(dtos);
        }

        /// <summary>
        /// Retrieves an enrollment by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("ID is required.");

            var enrollment = _enrollmentService.GetEnrollmentById(id);
            if (enrollment == null) return NotFound();
            return Ok(enrollment);
        }

        /// <summary>
        /// Creates a new enrollment. enrollment id is generated automatically.
        /// </summary>
        /// <param name="enrollment"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] Enrollment enrollment)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _enrollmentService.AddEnrollment(enrollment);
                return CreatedAtAction(nameof(GetById), new { id = enrollment.Id }, enrollment);
            }
            catch (Exception ex)
            {
                // Log the exception (if logging is set up)
                return BadRequest($"Error creating enrollment: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing enrollment by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enrollment"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Enrollment enrollment)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("ID is required.");

            var result = _enrollmentService.UpdateEnrollment(id, enrollment);
            if (!result) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Deletes an enrollment by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest("ID is required.");

            var result = _enrollmentService.DeleteEnrollment(id);
            if (!result) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Retrieves enrollments by student email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("by-email")]
        public IActionResult GetEnrollmentsByStudentEmail([FromQuery]  string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required.");

            var enrollments = _enrollmentService.GetEnrollmentsByStudentEmail(email);
            if (enrollments == null || !enrollments.Any()) return NotFound();
            return Ok(enrollments);
        }

        /// <summary>
        /// Retrieves enrollments by student ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("by-student-id/{id}")]
        public IActionResult GetEnrollmentsByStudentId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("id is required.");

            var enrollments = _enrollmentService.GetEnrollmentsByStudentId(id);
            if (enrollments == null || !enrollments.Any()) return NotFound();
            return Ok(enrollments);
        }

        /// <summary>
        /// Retrieves enrollments by course ID.
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpGet("course/{courseId}")]
        public ActionResult<IEnumerable<Enrollment>> GetEnrollmentsByCourseId(string courseId)
        {
            var enrollments = _enrollmentService.GetEnrollmentsByCourseId(courseId);

            if (enrollments == null || !enrollments.Any())
                return NotFound($"No enrollments found for course ID: {courseId}");

            return Ok(enrollments);
        }
    }
}