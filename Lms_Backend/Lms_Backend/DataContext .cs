using Lms_Backend.Interfaces;
using Lms_Backend.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Lms_Backend
{
    /// <summary>
    /// Represents the application database context - later should be using real DB.
    /// </summary>
    public class DataContext:IDataContext
    {
        public ConcurrentDictionary<string, Student> Students { get; } = new ConcurrentDictionary<string, Student>();
        public ConcurrentDictionary<string, Course> Courses { get; } = new ConcurrentDictionary<string, Course>();
        public ConcurrentDictionary<string, Enrollment> Enrollments { get; } = new ConcurrentDictionary<string, Enrollment>();
    }
}