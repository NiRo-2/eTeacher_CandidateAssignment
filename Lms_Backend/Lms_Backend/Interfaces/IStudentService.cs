using Lms_Backend.Models;

namespace Lms_Backend.Interfaces
{
    public interface IStudentService
    {
        List<Student> GetAllStudents();
        Student? GetStudentById(string id);
        Student? GetStudentByEmail(string email);
        void AddStudent(Student student);
        bool UpdateStudent(string id, Student student);
        bool DeleteStudent(string id);
    }
}