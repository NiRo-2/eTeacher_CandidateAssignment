import React, { useEffect, useState } from 'react';
import { fetchCourses } from '../api/apiService';

export default function CourseList() {
  const [courses, setCourses] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchCourses()
      .then(setCourses)
      .catch(err => setError(err.message));
  }, []);

  if (error) return <p className="text-red-500">Error: {error}</p>;

  return (
    <div>
      <h2 className="text-xl font-semibold mb-4">Courses</h2>
      {courses.length === 0 ? (
        <p>No courses available.</p>
      ) : (
        <ul className="list-disc pl-5">
          {courses.map(course => (
            <li key={course.id}>{course.name} - {course.description}</li>
          ))}
        </ul>
      )}
    </div>
  );
}