using Lms_Backend.Interfaces;
using Lms_Backend.Models;
using Lms_Backend.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Concurrent;

namespace Lms_Backend.Tests;

public class CourseServiceTests
{
    [Fact]
    public void AddCourse_EmptyName_ThrowsArgumentException()
    {
        var context = new Mock<IDataContext>();
        context.Setup(c => c.Courses).Returns(new ConcurrentDictionary<string, Course>());
        var service = new CourseService(Mock.Of<ILogger<CourseService>>(), context.Object);

        var invalidCourse = new Course
        {
            Name = "",  // Invalid name
            Description = "A course with no name"
        };

        Assert.Throws<ArgumentException>(() => service.AddCourse(invalidCourse));
    }
}
