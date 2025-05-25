using Lms_Backend.Interfaces;
using Lms_Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lms_Backend.Controllers
{
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
        /// Retrieves an enrollment by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var enrollment = _enrollmentService.GetEntorllmentById(id);
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
            //TODO validate enrollment data (e.g., check if student and course exist)
            _enrollmentService.AddEnrollment(enrollment);
            return CreatedAtAction(nameof(GetById), new { id = enrollment.Id }, enrollment);
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
            var result = _enrollmentService.DeleteEnrollment(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}