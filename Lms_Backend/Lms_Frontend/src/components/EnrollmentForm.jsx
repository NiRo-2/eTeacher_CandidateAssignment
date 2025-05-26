import React, { useEffect, useState } from 'react';
import { fetchEnrollments } from '../api/apiService';

export default function EnrollmentList() {
  const [enrollments, setEnrollment] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchEnrollments()
      .then(setEnrollment)
      .catch(err => setError(err.message));
  }, []);

  if (error) return <p className="text-red-500">Error: {error}</p>;

  return (
    <div>
      <h2 className="text-xl font-semibold mb-4">Enrollments</h2>
      {enrollments.length === 0 ? (
        <p>No enrollments available.</p>
      ) : (
        <ul className="list-disc pl-5">
          {enrollments.map(enrollment => (
            //TODO Format enrollment.enrolledAt to a readable date format
            //TODO get student data and course data for each enrollment
            <li key={enrollment.id}>enrolledAt: {enrollment.enrolledAt}</li>
          ))}
        </ul>
      )}
    </div>
  );
}