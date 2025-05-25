using System.Text.Json.Serialization;

namespace Lms_Backend.Models
{
    public class Enrollment
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StudentId { get; set; } = "";
        public string CourseId { get; set; } = "";
        public DateTimeOffset EnrolledAt { get; set; } = DateTime.Now;
    }
}