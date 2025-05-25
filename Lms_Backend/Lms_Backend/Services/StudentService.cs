using Lms_Backend.Interfaces;
using Lms_Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace Lms_Backend.Services
{
    public class StudentService : IStudentService
    {
        private readonly ILogger<StudentService> _logger;
        private readonly IDataContext _context;

        public StudentService(ILogger<StudentService> logger, IDataContext dataContext)
        {
            _logger = logger;
            _context = dataContext;
        }

        /// <summary>
        /// Retrieves all students in the system.
        /// </summary>
        /// <returns></returns>
        public List<Student> GetAllStudents() => _context.Students.Values.ToList();

        /// <summary>
        /// Retrieves a student by their ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return null incase of not found</returns>
        public Student? GetStudentById(string id)
        {
            _context.Students.TryGetValue(id, out var student);
            return student;
        }

        /// <summary>
        /// Retrieves a student by their email address.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Student? GetStudentByEmail(string email)
        {
            return _context.Students.Values.FirstOrDefault(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Adds a new student to the system
        /// </summary>
        /// <param name="student"></param>
        public void AddStudent(Student student)
        {
            student.Id = Guid.NewGuid().ToString();

            //validate email address
            if (!new EmailAddressAttribute().IsValid((student.Email)))
                throw new ArgumentException("Invalid email address format.");

            //Making sure email is unique (not exists by another student)
            if (_context.Students.Values.Any(s => s.Email.Equals(student.Email, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("A student with the same email already exists.");

            _context.Students[student.Id] = student;
        }

        /// <summary>
        /// Updates an existing student by their ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        public bool UpdateStudent(string id, Student student)
        {
            if (!_context.Students.ContainsKey(id)) return false;

            //validate email address
            if(!new EmailAddressAttribute().IsValid((student.Email)))
                throw new ArgumentException("Invalid email address format.");

            // Check if the new email is already used by another student (except current one)
            bool emailInUse = _context.Students
                .Where(kvp => kvp.Key != id)
                .Any(kvp => kvp.Value.Email.Equals(student.Email, StringComparison.OrdinalIgnoreCase));

            if (emailInUse) //updated email found - don't allow update
                throw new InvalidOperationException("A student with the same email already exists.");

            //all good - update student details
            _context.Students[id].FirstName = student.FirstName;
            _context.Students[id].LastName = student.LastName;
            _context.Students[id].Email = student.Email;

            return true;
        }

        /// <summary>
        /// Deletes a student by their ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteStudent(string id)
        {
            //**Note** for now instead of archiving/removing all related enrollments, just log a warning of x orphan enrollments stay on db
            int orphanEnrollments = _context.Enrollments.Values.Count(e => e.StudentId == id);
            _logger.LogWarning($"Student id: {id} has {orphanEnrollments} orphan enrollments that will not be deleted.");

            return _context.Students.TryRemove(id,out _);
        }
    }
}