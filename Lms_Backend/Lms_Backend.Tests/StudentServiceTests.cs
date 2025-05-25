using Lms_Backend.Interfaces;
using Lms_Backend.Models;
using Lms_Backend.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Concurrent;

namespace Lms_Backend.Tests
{
    public class StudentServiceTests
    {
        private readonly StudentService _service;
        private readonly Mock<IDataContext> _mockContext;
        private readonly Mock<ILogger<StudentService>> _mockLogger;

        //Data memory store for Students
        private readonly ConcurrentDictionary<string, Student> _students;

        public StudentServiceTests()
        {
            _mockLogger = new Mock<ILogger<StudentService>>();
            _mockContext = new Mock<IDataContext>();

            // Setup in-memory store for Students
            _students = new ConcurrentDictionary<string, Student>();

            // Setup the context mock to return the in-memory dictionaries
            _mockContext.Setup(c => c.Students).Returns(_students);
            _mockContext.Setup(c => c.Enrollments).Returns(new ConcurrentDictionary<string, Models.Enrollment>());

            _service = new StudentService(_mockLogger.Object, _mockContext.Object);
        }

        //adding a student with valid data
        [Fact]
        public void AddStudent_ValidStudent_AddsSuccessfully()
        {
            var student = new Student
            {
                FirstName = "Darda",
                LastName = "Saba",
                Email = "Dadasabal@example.com"
            };

            _service.AddStudent(student);

            Assert.Single(_students);
            var addedStudent = _students.Values.First();
            Assert.Equal(student.FirstName, addedStudent.FirstName);
            Assert.Equal(student.LastName, addedStudent.LastName);
            Assert.Equal(student.Email, addedStudent.Email);
            Assert.False(string.IsNullOrEmpty(addedStudent.Id));
        }

        // Test for adding a student with an existing email
        [Fact]
        public void AddStudent_InvalidEmail_ThrowsArgumentException()
        {
            var student = new Student
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "invalid-email"
            };

            Assert.Throws<ArgumentException>(() => _service.AddStudent(student));
        }

        //add student and retrieve by email
        [Fact]
        public void GetStudentByEmail_ExistingEmail_ReturnsStudent()
        {
            var student = new Student
            {
                Id = "1",
                FirstName = "Alice",
                LastName = "Brown",
                Email = "alice.brown@example.com"
            };
            _students[student.Id] = student;

            var result = _service.GetStudentByEmail("alice.brown@example.com");

            Assert.NotNull(result);
            Assert.Equal(student.Id, result.Id);
        }

        // Test for retrieving a student by non-existing email
        [Fact]
        public void GetStudentByEmail_NonExistingEmail_ReturnsNull()
        {
            var result = _service.GetStudentByEmail("nonexistent@example.com");

            Assert.Null(result);
        }

        //add new student, update it and check if the update was successful
        [Fact]
        public void UpdateStudent_ValidUpdate_ReturnsTrue()
        {
            var student = new Student
            {
                Id = "123",
                FirstName = "Mark",
                LastName = "Twain",
                Email = "mark.twain@example.com"
            };
            _students[student.Id] = student;

            var updatedStudent = new Student
            {
                FirstName = "Marcus",
                LastName = "Twain",
                Email = "marcus.twain@example.com"
            };

            var result = _service.UpdateStudent(student.Id, updatedStudent);

            Assert.True(result);
            Assert.Equal(updatedStudent.FirstName, _students[student.Id].FirstName);
            Assert.Equal(updatedStudent.Email, _students[student.Id].Email);
        }

        //add student and update with invalid email
        [Fact]
        public void UpdateStudent_InvalidEmail_ThrowsArgumentException()
        {
            var student = new Student
            {
                Id = "456",
                FirstName = "Sara",
                LastName = "Connor",
                Email = "sara.connor@example.com"
            };
            _students[student.Id] = student;

            var updatedStudent = new Student
            {
                FirstName = "Sara",
                LastName = "Connor",
                Email = "bad-email"
            };

            Assert.Throws<ArgumentException>(() => _service.UpdateStudent(student.Id, updatedStudent));
        }

        //remove student and check if it was removed successfully
        [Fact]
        public void DeleteStudent_ExistingId_ReturnsTrue()
        {
            var student = new Student
            {
                Id = "789",
                FirstName = "James",
                LastName = "Bond",
                Email = "james.bond@example.com"
            };
            _students[student.Id] = student;

            var result = _service.DeleteStudent(student.Id);

            Assert.True(result);
            Assert.False(_students.ContainsKey(student.Id));
        }

        //remove student with non-existing id
        [Fact]
        public void DeleteStudent_NonExistingId_ReturnsFalse()
        {
            var result = _service.DeleteStudent("non-existent-id");

            Assert.False(result);
        }
    }
}