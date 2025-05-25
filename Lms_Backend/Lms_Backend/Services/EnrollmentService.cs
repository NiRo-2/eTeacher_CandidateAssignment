using Lms_Backend.Interfaces;
using Lms_Backend.Models;

namespace Lms_Backend.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly Dictionary<string, Enrollment> _enrollments = new Dictionary<string, Enrollment>();

        // Get all enrollments
        public List<Enrollment> GetAllEnrollments() => _enrollments.Values.ToList();

        /// <summary>
        /// Retrieves an enrollment by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>null if not found</returns>
        public Enrollment? GetEntorllmentById(string id)
        {
            if (_enrollments.TryGetValue(id, out var enrollment))
                return enrollment;
            return null;
        }

        /// <summary>
        /// Adds a new enrollment
        /// </summary>
        /// <param name="enrollment"></param>
        public void AddEnrollment(Enrollment enrollment)
        {
            enrollment.Id = Guid.NewGuid().ToString(); // Ensure a new ID is generated

            //TODO validate enrollment data (e.g., check if student and course exist)
            //TODO validate course capacity - not to exceed max capacity
            _enrollments[enrollment.Id] = enrollment;
        }

        /// <summary>
        /// Updates an existing enrollment by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedEnrollment"></param>
        /// <returns></returns>
        public bool UpdateEnrollment(string id, Enrollment updatedEnrollment)
        {
            if (!_enrollments.ContainsKey(id)) return false;

            _enrollments[id].StudentId = updatedEnrollment.StudentId;
            _enrollments[id].CourseId = updatedEnrollment.CourseId;
            _enrollments[id].EnrolledAt = updatedEnrollment.EnrolledAt;
            return true;
        }

        /// <summary>
        /// Deletes an enrollment by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteEnrollment(string id)
        {
            return _enrollments.Remove(id);
        }
    }
}