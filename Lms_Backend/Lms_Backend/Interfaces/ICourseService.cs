using Lms_Backend.Models;

namespace Lms_Backend.Interfaces
{
    public interface ICourseService
    {
        List<Course> GetAllCourses();
        public List<CourseWithStudentsDto> GetAllCoursesWithStudents();
        Course? GetCourseById(string id);
        void AddCourse(Course course);
        bool UpdateCourse(string id, Course course);
        bool DeleteCourse(string id);
    }
}