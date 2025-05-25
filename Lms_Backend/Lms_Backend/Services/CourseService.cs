using Lms_Backend.Interfaces;
using Lms_Backend.Models;

namespace Lms_Backend.Services
{
    public class CourseService : ICourseService
    {
        private readonly IDataContext _context;
        private readonly ILogger<CourseService> _logger;
        public CourseService(ILogger<CourseService> logger,IDataContext dataContext)
        {
            _logger = logger;
            _context = dataContext;
        }

        //get all courses
        public List<Course> GetAllCourses() => _context.Courses.Values.ToList();

        /// <summary>
        /// Retrieves a course by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>null if not found</returns>
        public Course? GetCourseById(string id)
        {
            _context.Courses.TryGetValue(id, out var course);
            return course;
        }

        /// <summary>
        /// Adds a new course
        /// </summary>
        /// <param name="course"></param>
        public void AddCourse(Course course)
        {
            //validate no duplicate course names
            if(_context.Courses.Values.Any(c => c.Name.Equals(course.Name, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("A course with the same name already exists.");

            course.Id = Guid.NewGuid().ToString(); //generate a new ID for the course
            _context.Courses[course.Id] = course;
        }

        /// <summary>
        /// Updates an existing course by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedCourse"></param>
        /// <returns></returns>
        public bool UpdateCourse(string id, Course updatedCourse)
        {
            if (!_context.Courses.ContainsKey(id)) return false;

            //validate no duplicate course names
            if (_context.Courses.Values.Any(c => c.Name.Equals(updatedCourse.Name, StringComparison.OrdinalIgnoreCase) && c.Id != id))
                throw new ArgumentException("A course with the same name already exists.");

            // Update the course details
            _context.Courses[id].Name = updatedCourse.Name;
            _context.Courses[id].Description = updatedCourse.Description;
            _context.Courses[id].MaxCapacity = updatedCourse.MaxCapacity;
            return true;
        }

        /// <summary>
        /// Deletes a course by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteCourse(string id)
        {
            //**Note** for now just log warning of x orphan enrollments stay on db with ghost course
            int enrollmentCount = GetEnrollmentsByCourseId(id).Count;
            _logger.LogWarning($"{id} has {enrollmentCount} orphan enrollments that will not be deleted.");

            return _context.Courses.TryRemove(id, out _);
        }

        /// <summary>
        /// Get all enrollments for a course by its ID
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<Enrollment> GetEnrollmentsByCourseId(string courseId)
        {
            return _context.Enrollments.Values
                .Where(e => e.CourseId == courseId)
                .ToList();
        }
    }
}