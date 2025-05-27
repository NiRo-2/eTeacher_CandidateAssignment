import { BrowserRouter as Router, Routes, Route, NavLink } from 'react-router-dom';
import CourseList from './components/CourseList';
import StudentList from './components/StudentList';
import EnrollmentList from './components/EnrollmentList';
import CourseForm from './components/CourseForm';

export default function App() {
  return (
    <Router>
      <div className="min-h-screen flex bg-gray-50">
        {/* Sidebar */}
        <nav className="w-48 bg-gray-800 border-r p-6 flex flex-col gap-6 text-white">
          <h1 className="text-2xl font-bold mb-4">eTeacher LMS</h1>
          <NavLink
            to="/"
            end
            className={({ isActive }) =>
              `block px-3 py-2 rounded ${isActive ? 'bg-blue-600 text-white' : 'text-blue-600 hover:bg-blue-100'
              }`
            }
          >
            Courses
          </NavLink>
          <NavLink
            to="/students"
            className={({ isActive }) =>
              `block px-3 py-2 rounded ${isActive ? 'bg-blue-600 text-white' : 'text-blue-600 hover:bg-blue-100'
              }`
            }
          >
            Students
          </NavLink>
          <NavLink
            to="/enroll"
            className={({ isActive }) =>
              `block px-3 py-2 rounded ${isActive ? 'bg-blue-600 text-white' : 'text-blue-600 hover:bg-blue-100'
              }`
            }
          >
            Enrollments
          </NavLink>
        </nav>

        {/* Main Content Area */}
        <main className="flex-1 max-w-4xl mx-auto p-6 bg-gray-900 text-white">
          <Routes>
            <Route path="/" element={<CourseList />} />
            <Route path="/course/new" element={<CourseForm />} />
            <Route path="/course/edit/:id" element={<CourseForm />} />
            <Route path="/students" element={<StudentList />} />
            <Route path="/enroll" element={<EnrollmentList />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}