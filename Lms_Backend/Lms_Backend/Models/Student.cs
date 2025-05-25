using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lms_Backend.Models
{
    public class Student
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}