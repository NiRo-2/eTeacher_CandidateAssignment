using Lms_Backend.Models;

namespace Lms_Backend.Interfaces
{
    public interface IEnrollmentService
    {
        List<Enrollment> GetAllEnrollments();
        Enrollment? GetEntorllmentById(string id);
        void AddEnrollment(Enrollment enrollment);
        bool UpdateEnrollment(string id, Enrollment enrollment);
        bool DeleteEnrollment(string id);
    }
}
