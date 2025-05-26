using Lms_Backend.Interfaces;
using Lms_Backend.Models;

namespace Lms_Backend
{
    /// <summary>
    /// Represents the database seeder that populates the in-memory data context with initial data.
    /// </summary>
    public class DbSeeder
    {
        /// <summary>
        /// Seeds the in-memory database with initial data for students, courses, and enrollments.
        /// </summary>
        /// <param name="courseService"></param>
        /// <param name="enrollmentService"></param>
        /// <param name="studentService"></param>
        /// <exception cref="Exception"></exception>
        public static void Seed(ICourseService courseService,IEnrollmentService enrollmentService,IStudentService studentService)
        {
            // Seed students
            studentService.AddStudent(new Student() { FirstName = "Alice", LastName = "Blond", Email = "bla@na.com" });
            studentService.AddStudent(new Student() { FirstName = "Darda", LastName = "Saba", Email = "darda@saba.com" });
            // Verify seeding by checking the count of students(and get their IDs)
            List<Student> students = studentService.GetAllStudents();
            if (students.Count != 2) throw new Exception("Error while seeding students");

            // Seed courses
            courseService.AddCourse(new Course {Name = "Math", Description = "Desc1", MaxCapacity = 2 });
            courseService.AddCourse(new Course {Name = "History", Description = "Desc2", MaxCapacity = 2 });

            // Get all courses to verify seeding (and to get their IDs)
            List<Course> courses= courseService.GetAllCourses();
            if(courses.Count != 2) throw new Exception("Error while seeding courses");

            // Seed enrollments
            enrollmentService.AddEnrollment(new Enrollment { StudentId = students[0].Id, CourseId = courses[0].Id }); //student 0 enrolls in course 1 only
            enrollmentService.AddEnrollment(new Enrollment { StudentId = students[1].Id, CourseId = courses[0].Id }); //student 1 enrolls in course 1+2
            enrollmentService.AddEnrollment(new Enrollment { StudentId = students[1].Id, CourseId = courses[1].Id });

            // Verify seeding by checking the count of enrollments
            List<Enrollment> enrollments = enrollmentService.GetAllEnrollments();
            if (enrollments.Count != 3) throw new Exception("Error while seeding enrollments");
        }
    }
}