import { API_BASE_URL } from './config';

/**
 * 
 * @returns {Promise<Array>} Returns a promise that resolves to an array of courses.
 */
export async function fetchCoursesWithStudents() {
  const res = await fetch(`${API_BASE_URL}/course/with-students`);
  if (!res.ok) throw new Error('Failed to fetch courses (with students)');
  return res.json();
}

/**
 * 
 * @returns {Promise<Array>} Returns a promise that resolves to an array of enrollments.
 */
export async function fetchEnrollments_WithDetails() {
  const res = await fetch(`${API_BASE_URL}/enrollment/with-details`);
  if (!res.ok) throw new Error('Failed to fetch enrollments');
  return res.json();
}

/**
 * 
 * @returns {Promise<Array>} Returns a promise that resolves to an array of students.
 */
export async function fetchStudents() {
  const res = await fetch(`${API_BASE_URL}/student`);
  if (!res.ok) throw new Error('Failed to fetch students');
  return res.json();
}

/**
 * 
 * @param {*} course add new course
 * @returns 
 */
export async function addCourse(course) {
  const res = await fetch(`${API_BASE_URL}/course`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(course),
  });

  if (!res.ok) {
    // Try to parse error message from response
    let errorMessage = 'Failed to add course';
    try {
      const errorData = await res.json();
      if (errorData && errorData.message) {
        errorMessage = errorData.message;
      }
    } catch {
      // If parsing JSON fails, keep generic message
    }
    throw new Error(errorMessage);
  }

  return res.json();
}

/**
 * Get course by ID
 * @param {string|number} id 
 * @returns {Promise<Object>} Returns a promise resolving to the course object
 */
export async function getCourseById(id) {
  const res = await fetch(`${API_BASE_URL}/course/${id}`);
  if (!res.ok) throw new Error('Failed to fetch course');
  return res.json();
}

/**
 * Update existing course by ID
 * @param {string|number} id 
 * @param {Object} course 
 * @returns {Promise<Object>} Returns a promise resolving to the updated course object
 */
export async function editCourse(id, course) {
  const res = await fetch(`${API_BASE_URL}/course/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(course),
  });

  if (!res.ok) {
    let errorMessage = 'Failed to update course';
    try {
      const errorData = await res.json();
      if (errorData && errorData.message) {
        errorMessage = errorData.message;
      }
    } catch {
      // Ignore JSON parse errors and keep generic message
    }
    throw new Error(errorMessage);
  }

  // If 204 No Content, return nothing
  if (res.status === 204) return;

  // Otherwise, parse and return the updated course object
  return res.json();
}

/**
 * Assign a student to a course
 * @param {*} courseId course ID to assign the student to
 * @param {*} studentId student ID to assign
 * @returns 
 */
export async function assignStudentToCourse({ courseId, studentId }) {
  const res = await fetch(`${API_BASE_URL}/enrollment`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ courseId, studentId }),
  });

  if (!res.ok) {
    const errData = await res.json().catch(() => ({}));
    throw new Error(errData.message || 'Failed to assign student');
  }

  return res.json();
}