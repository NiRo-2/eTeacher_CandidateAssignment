using Lms_Backend.Interfaces;
using Lms_Backend.Models;
using Lms_Backend.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Concurrent;

namespace Lms_Backend.Tests;

public class EnrollmentServiceTests
{
    /// Test for adding an enrollment to a course that has reached its maximum capacity
    [Fact]
    public void AddEnrollment_NonExistingStudent_ThrowsInvalidOperationException()
    {
        var context = new Mock<IDataContext>();
        context.Setup(c => c.Students).Returns(new ConcurrentDictionary<string, Student>()); // no student
        context.Setup(c => c.Courses).Returns(new ConcurrentDictionary<string, Course>
        {
            ["c1"] = new Course { Id = "c1", Name = "Physics", MaxCapacity = 1 }
        });
        context.Setup(c => c.Enrollments).Returns(new ConcurrentDictionary<string, Enrollment>());

        var service = new EnrollmentService(Mock.Of<ILogger<EnrollmentService>>(), context.Object);

        var enrollment = new Enrollment { StudentId = "nonexistent", CourseId = "c1", EnrolledAt = DateTime.Now };

        Assert.Throws<InvalidOperationException>(() => service.AddEnrollment(enrollment));
    }

    /// Test for adding an enrollment to a course that has reached its maximum capacity
    [Fact]
    public void AddEnrollment_NonExistingCourse_ThrowsInvalidOperationException()
    {
        var context = new Mock<IDataContext>();
        context.Setup(c => c.Students).Returns(new ConcurrentDictionary<string, Student>
        {
            ["s1"] = new Student { Id = "s1", FirstName = "Alice", LastName = "Wong", Email = "alice@example.com" }
        });
        context.Setup(c => c.Courses).Returns(new ConcurrentDictionary<string, Course>()); // no course
        context.Setup(c => c.Enrollments).Returns(new ConcurrentDictionary<string, Enrollment>());

        var service = new EnrollmentService(Mock.Of<ILogger<EnrollmentService>>(), context.Object);

        var enrollment = new Enrollment { StudentId = "s1", CourseId = "nonexistent", EnrolledAt = DateTime.Now };

        Assert.Throws<InvalidOperationException>(() => service.AddEnrollment(enrollment));
    }

}