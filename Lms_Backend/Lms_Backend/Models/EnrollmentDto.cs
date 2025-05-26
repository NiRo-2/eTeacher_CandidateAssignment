namespace Lms_Backend.Models
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for enrollment details.
    /// </summary>
    public class EnrollmentDto
    {
        public string Id { get; set; } //enroillment ID
        public DateTime EnrolledAt { get; set; }

        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentEmail { get; set; }

        public string CourseName { get; set; }
    }
}