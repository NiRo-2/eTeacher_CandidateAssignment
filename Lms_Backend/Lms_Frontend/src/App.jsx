import { BrowserRouter as Router, Routes, Route, NavLink } from 'react-router-dom';
import CourseList from './components/CourseList';
import StudentList from './components/StudentList';
import EnrollmentForm from './components/EnrollmentForm';
import Report from './components/Report';

export default function App() {
  return (
    <Router>
      <div className="min-h-screen flex bg-gray-50">
        {/* Sidebar */}
        <nav className="w-48 bg-white border-r p-6 flex flex-col gap-6">
          <h1 className="text-2xl font-bold mb-4">eTeacher LMS</h1>
          <NavLink
            to="/"
            end
            className={({ isActive }) =>
              `block px-3 py-2 rounded ${
                isActive ? 'bg-blue-600 text-white' : 'text-blue-600 hover:bg-blue-100'
              }`
            }
          >
            Courses
          </NavLink>
          <NavLink
            to="/students"
            className={({ isActive }) =>
              `block px-3 py-2 rounded ${
                isActive ? 'bg-blue-600 text-white' : 'text-blue-600 hover:bg-blue-100'
              }`
            }
          >
            Students
          </NavLink>
          <NavLink
            to="/enroll"
            className={({ isActive }) =>
              `block px-3 py-2 rounded ${
                isActive ? 'bg-blue-600 text-white' : 'text-blue-600 hover:bg-blue-100'
              }`
            }
          >
            Enroll
          </NavLink>
          <NavLink
            to="/report"
            className={({ isActive }) =>
              `block px-3 py-2 rounded ${
                isActive ? 'bg-blue-600 text-white' : 'text-blue-600 hover:bg-blue-100'
              }`
            }
          >
            Report
          </NavLink>
        </nav>

        {/* Main Content Area */}
        <main className="flex-1 max-w-4xl mx-auto p-6">
          <Routes>
            <Route path="/" element={<CourseList />} />
            <Route path="/students" element={<StudentList />} />
            <Route path="/enroll" element={<EnrollmentForm />} />
            <Route path="/report" element={<Report />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}