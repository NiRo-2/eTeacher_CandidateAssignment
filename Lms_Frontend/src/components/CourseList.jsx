import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { fetchCoursesWithStudents } from '../api/apiService';
import AssignStudentModal from './AssignStudentModal'; // Make sure this file exists

// CourseList component to display a list of courses with options to add new courses and assign students
export default function CourseList() {
  const [courses, setCourses] = useState([]);
  const [error, setError] = useState(null);
  const [showAssignModal, setShowAssignModal] = useState(false);
  const [selectedCourseId, setSelectedCourseId] = useState(null);
  const selectedCourse = courses.find(c => c.id === selectedCourseId);

  useEffect(() => {
    loadCourses();
  }, []);

  // Function to fetch courses from the API
  async function loadCourses() {
    try {
      const data = await fetchCoursesWithStudents();
      setCourses(data);
    } catch (err) {
      setError(err.message || 'Failed to load courses');
    }
  }

  // Function to open the modal for assigning students to a course
  function openAssignModal(courseId) {
    // If clicking the same button again, toggle close
    if (showAssignModal && selectedCourseId === courseId) {
      closeAssignModal();
    } else {
      setSelectedCourseId(courseId);
      setShowAssignModal(true);
    }
  }

  // Function to close the modal and reset state
  function closeAssignModal() {
    setSelectedCourseId(null);
    setShowAssignModal(false);
  }

  return (
    <div className="max-w-3xl mx-auto p-4">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-bold">Courses</h1>
        <Link
          to="/course/new"
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          + Add New Course
        </Link>
      </div>

      {error && <p className="text-red-600 mb-3">{error}</p>}

      <ul className="list-disc pl-5 space-y-2">
        {courses.length === 0 && <li>No courses available.</li>}
        {courses.map(course => (
          <li key={course.id} className="flex justify-between items-center gap-4">
            <div>
              <strong>{course.name}</strong>
              {course.description && ` – ${course.description}`}
            </div>
            <div className="text-sm text-gray-600">
              Capacity: {course.students?.length ?? 0} / {course.maxCapacity}
            </div>
            <div className="flex gap-2">
              <Link
                to={`/course/edit/${course.id}`}
                className="px-2 py-1 text-sm text-blue-600 border border-blue-600 rounded hover:bg-blue-50"
              >
                Edit
              </Link>
              <button
                onClick={() => openAssignModal(course.id)}
                className="px-2 py-1 text-sm text-blue-600 border border-blue-600 rounded hover:bg-blue-50"
              >
                Assign Students
              </button>
            </div>
          </li>
        ))}
      </ul>

      {/* Modal for assigning students to a course */}
      {showAssignModal && (
        <AssignStudentModal
          courseId={selectedCourseId}
          enrolledStudentIds={selectedCourse?.students.map(s => s.id) ?? []}
          onClose={closeAssignModal}
          onAssigned={loadCourses}
          show={showAssignModal}
        />
      )}
    </div>
  );
}