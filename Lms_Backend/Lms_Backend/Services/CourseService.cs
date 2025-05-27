using Lms_Backend.Interfaces;
using Lms_Backend.Models;

namespace Lms_Backend.Services
{
    public class CourseService : ICourseService
    {
        private readonly IDataContext _context;
        private readonly ILogger<CourseService> _logger;
        public CourseService(ILogger<CourseService> logger, IDataContext dataContext)
        {
            _logger = logger;
            _context = dataContext;
        }

        //get all courses
        public List<Course> GetAllCourses() => _context.Courses.Values.ToList();

        /// <summary>
        /// Retrieves all courses along with their enrolled students
        /// </summary>
        /// <returns></returns>
        public List<CourseWithStudentsDto> GetAllCoursesWithStudents()
        {
            return _context.Courses.Values.Select(course =>
            {
                // Get the IDs of students enrolled in the course
                var enrolledStudentIds = _context.Enrollments.Values
                    .Where(e => e.CourseId == course.Id)
                    .Select(e => e.StudentId)
                    .ToList();

                // Retrieve students enrolled in the course
                var students = _context.Students.Values
                    .Where(s => enrolledStudentIds.Contains(s.Id))
                    .ToList();

                // Create a CourseWithStudentsDto to return the course details along with the enrolled students
                return new CourseWithStudentsDto
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                    MaxCapacity = course.MaxCapacity,
                    Students = students
                };
            }).ToList();
        }

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
            // Validate course properties
            if (string.IsNullOrWhiteSpace(course.Name)) throw new ArgumentException("Course name cannot be empty.");
            if (course.MaxCapacity <= 0) throw new ArgumentException("Max capacity must be greater than zero.");

            //validate no duplicate course names
            if (_context.Courses.Values.Any(c => c.Name.Equals(course.Name, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("A course with the same name already exists.");

            course.Id = Guid.NewGuid().ToString(); //generate a new ID for the course
            _context.Courses[course.Id] = course;

            // Log the addition of the course
            _logger.LogInformation($"Course added: {course.Name} (ID: {course.Id})");
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

            //validate max capacity will not be less than current enrollments
            int currentEnrollments = _context.Enrollments.Values.Count(e => e.CourseId == id);
            if(updatedCourse.MaxCapacity < currentEnrollments)
                throw new ArgumentException($"Max capacity cannot be less than the current number of enrollments ({currentEnrollments}).");

            //log course update
            _logger.LogInformation($"Updating course (ID: {id}): Name: {_context.Courses[id].Name}-->{updatedCourse.Name} Description: {_context.Courses[id].Description}-->{updatedCourse.Description} MaxCapacity: {_context.Courses[id].MaxCapacity}-->{updatedCourse.MaxCapacity}");

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
            int enrollmentCount = _context.Enrollments.Values.Count(e => e.CourseId == id);
            _logger.LogWarning($"{id} has {enrollmentCount} orphan enrollments that will not be deleted.");

            //log course deletion
            _logger.LogInformation($"Deleting course (ID: {id}): Name: {_context.Courses[id].Name}");
            return _context.Courses.TryRemove(id, out _);
        }
    }
}