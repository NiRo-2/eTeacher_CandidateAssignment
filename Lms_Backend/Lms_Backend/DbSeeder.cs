using Lms_Backend.Interfaces;
using Lms_Backend.Models;

namespace Lms_Backend
{
    /// <summary>
    /// Represents the database seeder that populates the in-memory data context with initial data.
    /// </summary>
    public class DbSeeder
    {
        public static void Seed(IDataContext context)
        {
            // Seed students
            bool added1 = context.Students.TryAdd("1", new Student { Id = "1", FirstName = "Alice", LastName = "Blond", Email = "bla@na.com" });
            bool added2 = context.Students.TryAdd("2", new Student { Id = "2", FirstName = "Darda", LastName = "Saba", Email = "darda@saba.com" });

            // Seed courses
            bool course1 = context.Courses.TryAdd("101", new Course { Id = "101", Name = "Math", Description = "Desc1", MaxCapacity = 2 });
            bool course2 = context.Courses.TryAdd("102", new Course { Id = "102", Name = "History", Description = "Desc2", MaxCapacity = 1 });

            // Seed enrollments
            bool enroll1 = context.Enrollments.TryAdd("1-101", new Enrollment { StudentId = "1", CourseId = "101" });
            bool enroll2 = context.Enrollments.TryAdd("2-102", new Enrollment { StudentId = "2", CourseId = "102" });
        }
    }
}