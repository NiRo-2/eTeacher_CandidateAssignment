# eTeacher_CandidateAssignment - LMS Dashboard

## ✅ Overview

A lightweight Learning Management System (LMS) for managing students, courses, and enrollments. Built with ASP.NET Core and an in-memory data store for simplicity and speed. ReactJS is used for the frontend UI.

## ✨ Features

- Manage Courses: View, Add, Edit, Delete  
- Manage Students: Add, Update, Delete  
- Enroll students in courses  
- View and manage enrollments  
- Generate enrollment reports (export to JSON, upload to AWS S3)  
- Basic unit tests with xUnit covering core functionalities  
- Uses UUIDs for safe and unique identifiers (non-sequential IDs)  

## 🧰 Tech Stack

- C# (ASP.NET Core) backend RESTful API  
- In-memory data storage using `ConcurrentDictionary`  
- xUnit for automated testing  
- React frontend (Simple React UI)  

## 🛠 Setup Instructions

### Clone the repository

```bash
git clone https://github.com/yourusername/eTeacher_CandidateAssignment.git
````

### Backend

```bash
cd Lms_Backend
dotnet run
```

### Frontend

```bash
cd Lms_Frontend
npm install
npm run dev
```

### Run Tests

```bash
cd Lms_Backend.Tests
dotnet test
```
