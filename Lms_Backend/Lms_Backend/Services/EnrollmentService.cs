using Lms_Backend.Interfaces;
using Lms_Backend.Models;

namespace Lms_Backend.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IDataContext _context;
        private readonly ILogger<EnrollmentService> _logger;

        public EnrollmentService(ILogger<EnrollmentService> logger,IDataContext dataContext)
        {
            _context = dataContext;
            _logger = logger;
        }

        // Get all enrollments
        public List<Enrollment> GetAllEnrollments() => _context.Enrollments.Values.ToList();

        /// <summary>
        /// Retrieves all enrollments as DTOs for easier consumption by clients
        /// </summary>
        /// <returns></returns>
        public List<EnrollmentDto> GetAllEnrollmentDtos()
        {
            var enrollments = _context.Enrollments.Values.ToList();
            var enrollmentDtos = enrollments.Select(e =>
            {
                _context.Students.TryGetValue(e.StudentId, out var student); //get student by ID
                _context.Courses.TryGetValue(e.CourseId, out var course); //get course by ID

                // Validate that both student and course exist
                if (student == null || course == null)
                    _logger.LogWarning("Enrollment {EnrollmentId} references non-existing student or course.", e.Id);

                // Create the DTO
                return new EnrollmentDto
                {
                    Id = e.Id,
                    EnrolledAt = e.EnrolledAt.ToLocalTime().DateTime,
                    StudentFirstName = student?.FirstName ?? "Unknown",
                    StudentLastName = student?.LastName ?? "Unknown",
                    StudentEmail = student?.Email ?? "Unknown",
                    CourseName = course?.Name ?? "Unknown"
                };
            }).ToList();

            return enrollmentDtos;
        }

        /// <summary>
        /// Retrieves an enrollment by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>null if not found</returns>
        public Enrollment? GetEnrollmentById(string id)
        {
            _context.Enrollments.TryGetValue(id, out var enrollment);
            return enrollment;
        }

        /// <summary>
        /// Adds a new enrollment
        /// </summary>
        /// <param name="enrollment"></param>
        public void AddEnrollment(Enrollment enrollment)
        {
            enrollment.Id = Guid.NewGuid().ToString(); // Ensure a new ID is generated

            //Validate student exists
            var student = _context.Students.Values.FirstOrDefault(s => s.Id == enrollment.StudentId);
            if (student == null)
                throw new InvalidOperationException("Student does not exist.");
            
            //Validate course exists
            var course = _context.Courses.Values.FirstOrDefault(c => c.Id == enrollment.CourseId);
            if (course == null)
                throw new InvalidOperationException("Course does not exist.");

            //Validate course capacity
            int currentEnrollmentCount = _context.Enrollments.Values.Count(e => e.CourseId == enrollment.CourseId); 
            if (currentEnrollmentCount >= course.MaxCapacity)
                throw new InvalidOperationException("Course has reached its maximum capacity.");

            //all good, add the enrollment
            enrollment.EnrolledAt = DateTimeOffset.Now;
            if (!_context.Enrollments.TryAdd(enrollment.Id, enrollment))
                throw new InvalidOperationException("Failed to add enrollment.");
        }

        /// <summary>
        /// Updates an existing enrollment by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enrollment"></param>
        /// <returns></returns>
        public bool UpdateEnrollment(string id, Enrollment enrollment)
        {
            if (!_context.Enrollments.ContainsKey(id)) return false;

            //validate student id exists
            var student = _context.Students.Values.FirstOrDefault(s => s.Id == enrollment.StudentId);
            if (student == null)
                throw new InvalidOperationException("Student does not exist.");

            //validate course id exists
            var course = _context.Courses.Values.FirstOrDefault(c => c.Id == enrollment.CourseId);
            if (course == null)
                throw new InvalidOperationException("Course does not exist.");

            //Validate course capacity
            int currentEnrollmentCount = _context.Enrollments.Values.Count(e => e.CourseId == enrollment.CourseId);
            if (currentEnrollmentCount >= course.MaxCapacity && _context.Enrollments[id].CourseId != enrollment.CourseId)
                throw new InvalidOperationException("Course has reached its maximum capacity.");

            // Update the enrollment details
            _context.Enrollments[id].StudentId = enrollment.StudentId;
            _context.Enrollments[id].CourseId = enrollment.CourseId;
            _context.Enrollments[id].EnrolledAt = enrollment.EnrolledAt;
            return true;
        }

        /// <summary>
        /// Get all enrollments for a specific student by their email address
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public List<Enrollment> GetEnrollmentsByStudentEmail(string email)
        {
            var student = _context.Students.Values.FirstOrDefault(s => s.Email == email);
            if (student == null)
            {
                _logger.LogWarning("No student found with email: {Email}", email);
                return new List<Enrollment>();
            }
            return GetEnrollmentsByStudentId(student.Id);
        }

        /// <summary>
        /// Get all enrollments for a specific student by their ID
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public List<Enrollment> GetEnrollmentsByStudentId(string studentId)
        {
            return _context.Enrollments.Values.Where(e => e.StudentId == studentId).ToList();
        }

        /// <summary>
        /// Deletes an enrollment by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteEnrollment(string id)
        {
            return _context.Enrollments.TryRemove(id,out _);
        }
    }
}