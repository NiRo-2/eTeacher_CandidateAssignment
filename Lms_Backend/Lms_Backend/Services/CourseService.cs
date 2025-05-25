using Lms_Backend.Interfaces;
using Lms_Backend.Models;

namespace Lms_Backend.Services
{
    public class CourseService:ICourseService
    {
        private readonly Dictionary<string, Course> _courses = new Dictionary<string, Course>();
        //get all courses
        public List<Course> GetAllCourses() => _courses.Values.ToList();

        /// <summary>
        /// Retrieves a course by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>null if not found</returns>
        public Course? GetCourseById(string id)
        {
            if (_courses.TryGetValue(id, out var course))
                return course;
            return null; //return null incase of not found
        }

        /// <summary>
        /// Adds a new course
        /// </summary>
        /// <param name="course"></param>
        public void AddCourse(Course course)
        {
            course.Id = Guid.NewGuid().ToString(); //generate a new ID for the course
            _courses[course.Id] = course;
        }

        /// <summary>
        /// Updates an existing course by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedCourse"></param>
        /// <returns></returns>
        public bool UpdateCourse(string id, Course updatedCourse)
        {
            if (!_courses.ContainsKey(id)) return false;

            _courses[id].Name = updatedCourse.Name;
            _courses[id].Description = updatedCourse.Description;
            _courses[id].MaxCapacity = updatedCourse.MaxCapacity;
            return true;
        }

        /// <summary>
        /// Deletes a course by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteCourse(string id)
        {
            return _courses.Remove(id);
        }
    }
}