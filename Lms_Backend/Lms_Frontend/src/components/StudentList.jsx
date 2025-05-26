import React, { useEffect, useState } from 'react';
import { fetchStudents } from '../api/apiService';

export default function StudentList() {
  const [students, setStudents] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchStudents()
      .then(setStudents)
      .catch(err => setError(err.message));
  }, []);

  if (error) return <p className="text-red-500">Error: {error}</p>;

  return (
    <div>
      <h2 className="text-xl font-semibold mb-4">Students:</h2>
      {students.length === 0 ? (
        <p>No students found.</p>
      ) : (
        <ul className="list-disc pl-5">
          {students.map(student => (
            <li key={student.id}>
              Name: {student.firstName} {student.lastName}, Email: {student.email}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}