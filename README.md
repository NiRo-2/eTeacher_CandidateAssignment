eTeacher_CandidateAssignment - LMS Dashboard
? Overview

A lightweight Learning Management System (LMS) for managing students, courses, and enrollments. Built with ASP.NET Core and using an in-memory data store for simplicity and speed, ReactJS for UI.
?? Features

    Manage Courses: View, Add, Edit, Delete
    
    Manage Students: Add, Update, Delete
    Enroll students in courses
    View and manage enrollments
    Generate enrollment reports (export to JSON, upload to AWS S3)
    Basic unit tests with xUnit covering core functionalities
    Uses UUIDs for safe and unique identifiers (non-sequential IDs)

?? Tech Stack

    C# (ASP.NET Core) backend RESTful API
    In-memory data storage using ConcurrentDictionary
    xUnit for automated testing
    React frontend (Simple React UI)

??? Setup Instructions
*Clone the repository*

Backend:
	
    Navigate to the backend directory:
		cd Lms_Backend
	Run the backend API:
		dotnet run
		
Frontend:
    Navigate to the frontend directory:
		cd Lms_Frontend
	Start the React development server:
		npm install
		npm run dev
		
Tests:
	Navigate to the tests directory:
		cd Lms_Backend.Tests
	Run all unit tests:
		dotnet test
