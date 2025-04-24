
# 📚 Online Library Management System

An ASP.NET Core MVC web application designed to streamline library operations, including book rentals, returns, and user management.

## 📝 Table of Contents
- [About the Project](#about-the-project)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Contributors](#contributors)
- [License](#license)

## 📖 About the Project
The Online Library Management System facilitates efficient management of library resources. It allows administrators to oversee book inventories, librarians to manage book rentals, and readers to borrow and return books seamlessly.

## ✨ Features
- User Authentication & Authorization
- Role-Based Access Control (Admin, Librarian, Reader)
- Book Catalog Management
- Book Rental and Return Functionality
- Late Fee and Fine Calculation
- Responsive UI with Bootstrap
- DataTables Integration for Enhanced Table Features

## 🛠️ Tech Stack
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap 5, jQuery, DataTables
- **Backend**: ASP.NET Core MVC, Entity Framework Core
- **Database**: SQL Server
- **Version Control**: Git & GitHub

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2019 or later
- .NET 5.0 SDK or later
- SQL Server

### Installation

1. **Clone the Repository**

```bash
git clone https://github.com/Jay-Dalsaniya/OnlineLibraryManagementSystem.git
```

2. **Open the Project**

Open the solution file `OnlineLibraryManagementSystem.sln` in Visual Studio.

3. **Configure the Database**

Update the `appsettings.json` file with your SQL Server connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=LibraryDB;Trusted_Connection=True;"
}
```

4. **Apply Migrations**

Open the Package Manager Console and run:

```bash
Update-Database
```

5. **Run the Application**

Press `F5` or click on the "Start" button in Visual Studio to run the application.


## 👥 Contributors
- Jay Dalsaniya

## 📄 License
This project is licensed under the [MIT License](LICENSE).
