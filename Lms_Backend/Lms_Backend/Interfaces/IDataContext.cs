using Lms_Backend.Models;
using System.Collections.Concurrent;

namespace Lms_Backend.Interfaces
{
    public interface IDataContext
    {
        ConcurrentDictionary<string, Student> Students { get; }
        ConcurrentDictionary<string, Course> Courses { get; }
        ConcurrentDictionary<string, Enrollment> Enrollments { get; }
    }
}