import React, { useEffect, useState } from 'react';
import { fetchEnrollments_WithDetails } from '../api/apiService';

// EnrollmentList component to display a list of enrollments with student and course details
export default function EnrollmentList() {
  const [enrollments, setEnrollments] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchEnrollments_WithDetails()
      .then(data => setEnrollments(data))
      .catch(err => setError(err.message));
  }, []);

  if (error) return <p className="text-red-500">Error: {error}</p>;

  // Function to format date strings into a more readable format
  const formatDate = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleDateString(undefined, {
      year: 'numeric',
      month: 'numeric',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  return (
    <div>
      <h2 className="text-xl font-semibold mb-4">Enrollments</h2>
      {enrollments.length === 0 ? (
        <p>No enrollments available.</p>
      ) : (
        <ul className="list-disc pl-5">
          {enrollments.map(enrollment => (
            // Display each enrollment with student and course details - Name, email, course name, and enrollment date
            <li key={enrollment.id} className="mb-2">
              <strong>{enrollment.studentFirstName} {enrollment.studentLastName}</strong> (
              {enrollment.studentEmail}) enrolled in <em>{enrollment.courseName}</em> on{' '}
              {formatDate(enrollment.enrolledAt)}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}