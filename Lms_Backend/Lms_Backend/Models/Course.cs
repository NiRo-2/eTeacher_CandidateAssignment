using System.Text.Json.Serialization;

namespace Lms_Backend.Models
{
    public class Course
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int MaxCapacity { get; set; }
    }

    /// <summary>
    /// Represents a course with a list of enrolled students.
    /// </summary>
    public class CourseWithStudentsDto : Course
    {
        public List<Student> Students { get; set; } = new List<Student>();
    }
}