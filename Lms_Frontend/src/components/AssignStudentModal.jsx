import React, { useEffect, useState } from 'react';
import { Modal, Button, Spinner } from 'react-bootstrap';
import { fetchStudents, assignStudentToCourse } from '../api/apiService';

//AssignStudentModal component to assign students to a course
export default function AssignStudentModal({ courseId, enrolledStudentIds = [], onClose, onAssigned, show }) {
    const [students, setStudents] = useState([]);
    const [loading, setLoading] = useState(true);
    const [assigning, setAssigning] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        if (!show) return; // only fetch when shown
        setLoading(true);
        fetchStudents()
            .then(setStudents)
            .catch(err => setError(err.message))
            .finally(() => setLoading(false));
    }, [show]);

    async function handleAssign(studentId) {
        setAssigning(true);
        try {
            await assignStudentToCourse({ courseId, studentId });
            onAssigned?.();
            onClose();
        } catch (err) {
            setError(err.message);
        } finally {
            setAssigning(false);
        }
    }

    return (
        <Modal show={show} onHide={onClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>Assign Student to Course</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {loading ? (
                    <div className="d-flex justify-content-center">
                        <Spinner animation="border" />
                    </div>
                ) : error ? (
                    <p className="text-danger">{error}</p>
                ) : (
                    <ul className="list-unstyled" style={{ maxHeight: '300px', overflowY: 'auto' }}>
                        {students.map(student => {
                            const alreadyAssigned = enrolledStudentIds.includes(student.id);
                            return (
                                <li key={student.id} className="d-flex justify-content-between align-items-center mb-2">
                                    <span>
                                        {student.firstName} {student.lastName} ({student.email})
                                        {alreadyAssigned && <span className="text-success ms-2">(Already Assigned)</span>}
                                    </span>
                                    <Button
                                        variant={alreadyAssigned ? "secondary" : "link"}
                                        disabled={assigning || alreadyAssigned}
                                        onClick={() => handleAssign(student.id)}
                                    >
                                        {alreadyAssigned ? "Assigned" : "Assign"}
                                    </Button>
                                </li>
                            );
                        })}
                    </ul>
                )}
            </Modal.Body>
        </Modal>
    );
}