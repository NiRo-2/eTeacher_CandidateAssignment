import { API_BASE_URL } from './config';

/**
 * 
 * @returns {Promise<Array>} Returns a promise that resolves to an array of courses.
 */
export async function fetchCourses() {
  const res = await fetch(`${API_BASE_URL}/courses`);
  if (!res.ok) throw new Error('Failed to fetch courses');
  return res.json();
}

/**
 * 
 * @returns {Promise<Array>} Returns a promise that resolves to an array of students.
 */
export async function fetchStudents() {
  const res = await fetch(`${API_BASE_URL}/students`);
  if (!res.ok) throw new Error('Failed to fetch students');
  return res.json();
}

//TODO create course class
/**
 * 
 * @param {*} course add new course
 * @returns 
 */
export async function addCourse(course) {
  const res = await fetch(`${API_BASE_URL}/courses`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(course),
  });
  if (!res.ok) throw new Error('Failed to add course');
  return res.json();
}

// TODO Add other API methods similarly (editCourse, enrollStudent, etc.)