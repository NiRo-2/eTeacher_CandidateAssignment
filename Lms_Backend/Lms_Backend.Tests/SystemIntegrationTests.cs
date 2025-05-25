using Lms_Backend.Interfaces;
using Lms_Backend.Models;
using Lms_Backend.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Concurrent;

namespace Lms_Backend.Tests
{
    public class SystemIntegrationTests
    {
        //setup temp data context for testing
        private IDataContext SetupDataContext()
        {
            var students = new ConcurrentDictionary<string, Student>();
            var courses = new ConcurrentDictionary<string, Course>();
            var enrollments = new ConcurrentDictionary<string, Enrollment>();

            var context = new Mock<IDataContext>();
            context.Setup(c => c.Students).Returns(students);
            context.Setup(c => c.Courses).Returns(courses);
            context.Setup(c => c.Enrollments).Returns(enrollments);

            return context.Object;
        }

        private ILogger<T> CreateLogger<T>()
        {
            return new Mock<ILogger<T>>().Object;
        }

        //do full system workflow test
        [Fact]
        public void FullSystemWorkflowTest()
        {
            // Arrange
            var context = SetupDataContext();
            var studentService = new StudentService(CreateLogger<StudentService>(), context);
            var courseService = new CourseService(CreateLogger<CourseService>(), context);
            var enrollmentService = new EnrollmentService(CreateLogger<EnrollmentService>(), context);

            // 1. Add students
            var student1 = new Student { FirstName = "Alice", LastName = "Smith", Email = "alice@example.com" };
            var student2 = new Student { FirstName = "Bob", LastName = "Jones", Email = "bob@example.com" };
            studentService.AddStudent(student1);
            studentService.AddStudent(student2);

            // 2. Add courses
            var course1 = new Course { Name = "Math 101", Description = "Basic Mathematics" ,MaxCapacity=1};
            var course2 = new Course { Name = "History 201", Description = "World History", MaxCapacity = 2 };
            courseService.AddCourse(course1);
            courseService.AddCourse(course2);

            // 3. Enroll students
            var enrollment1 = new Enrollment
            {
                StudentId = student1.Id,
                CourseId = course1.Id,
                EnrolledAt = DateTime.Now
            };
            var enrollment2 = new Enrollment
            {
                StudentId = student2.Id,
                CourseId = course2.Id,
                EnrolledAt = DateTime.Now
            };

            // Add enrollments
            enrollmentService.AddEnrollment(enrollment1);
            enrollmentService.AddEnrollment(enrollment2);

            // 4. Validate students stored correctly
            var allStudents = studentService.GetAllStudents();
            Assert.Equal(2, allStudents.Count);

            // 5. Validate courses stored correctly
            var allCourses = courseService.GetAllCourses();
            Assert.Equal(2, allCourses.Count);

            // 6. Validate enrollments by student
            var aliceEnrollments = enrollmentService.GetEnrollmentsByStudentId(student1.Id);
            Assert.Single(aliceEnrollments);
            Assert.Equal(course1.Id, aliceEnrollments[0].CourseId);

            // 7. Update a student email and check update
            student1.Email = "alice.new@example.com";
            bool updateResult = studentService.UpdateStudent(student1.Id, student1);
            Assert.True(updateResult);
            var updatedStudent = studentService.GetStudentById(student1.Id);
            Assert.Equal("alice.new@example.com", updatedStudent.Email);

            // 8. Delete a course and verify removal
            bool courseDeleteResult = courseService.DeleteCourse(course2.Id);
            Assert.True(courseDeleteResult);
            var coursesAfterDelete = courseService.GetAllCourses();
            Assert.Single(coursesAfterDelete);

            // 9. Delete a student and check
            bool studentDeleteResult = studentService.DeleteStudent(student2.Id);
            Assert.True(studentDeleteResult);
            var studentsAfterDelete = studentService.GetAllStudents();
            Assert.Single(studentsAfterDelete);

            // 10. Cleanup: Delete enrollment
            bool enrollmentDeleteResult = enrollmentService.DeleteEnrollment(enrollment1.Id);
            Assert.True(enrollmentDeleteResult);

            // Final check: no enrollments left
            var remainingEnrollments = enrollmentService.GetEnrollmentsByStudentId(student1.Id);
            Assert.Empty(remainingEnrollments);
        }
    }
}