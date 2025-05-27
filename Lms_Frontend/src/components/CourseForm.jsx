import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { getCourseById, addCourse, editCourse } from '../api/apiService';

export default function CourseForm() {
    const { id } = useParams(); // will be undefined on "new"
    const navigate = useNavigate();

    const [course, setCourse] = useState({ name: '', description: '', maxcapacity: 1 }); // default values
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    //fetch course data if editing
    useEffect(() => {
        if (id) {
            setLoading(true);
            getCourseById(id)
                .then(data => {                   
                    setCourse({
                        name: data.name || '',
                        description: data.description || '',
                        maxcapacity: data.maxCapacity > 0 ? data.maxCapacity : 1
                    });
                })
                .catch(err => setError(err.message))
                .finally(() => setLoading(false));
        }
    }, [id]);

    function handleChange(e) {
        const { name, value } = e.target;
    
        if (name === 'maxcapacity') {
            const intVal = parseInt(value, 10);
            setCourse(prev => ({
                ...prev,
                [name]: isNaN(intVal) || intVal <= 0 ? '' : intVal
            }));
        } else {
            setCourse(prev => ({ ...prev, [name]: value }));
        }
    }

    //handle form submission
    function handleSubmit(e) {
        e.preventDefault();

        const action = id ? editCourse(id, course) : addCourse(course);

        action
            .then(() => navigate('/'))
            .catch(err => setError(err.message));
    }

    if (loading) return <p>Loading course data...</p>;
    if (error) return <p className="text-red-600">{error}</p>;

    return (
        <div>
            <h2 className="text-xl font-bold mb-4">{id ? 'Edit Course' : 'Add New Course'}</h2>
            <form onSubmit={handleSubmit} className="space-y-4 max-w-lg">
                <div>
                    <label className="block mb-1 font-semibold" htmlFor="name">Course Name</label>
                    <input
                        id="name"
                        name="name"
                        type="text"
                        value={course.name}
                        onChange={handleChange}
                        required
                        className="w-full border px-3 py-2 rounded"
                    />
                </div>

                <div>
                    <label className="block mb-1 font-semibold" htmlFor="description">Description</label>
                    <textarea
                        id="description"
                        name="description"
                        value={course.description}
                        onChange={handleChange}
                        rows={3}
                        className="w-full border px-3 py-2 rounded"
                    />
                </div>

                <div>
                    <label className="block mb-1 font-semibold" htmlFor="maxcapacity">Max capacity</label>
                    <input
                        type="number"
                        id="maxcapacity"
                        name="maxcapacity"
                        value={course.maxcapacity}
                        onChange={handleChange}
                        min={1}
                        className="w-full border px-3 py-2 rounded"
                    />
                </div>

                <button
                    type="submit"
                    className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
                >
                    {id ? 'Update Course' : 'Add Course'}
                </button>
            </form>
        </div>
    );
}