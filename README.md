📚 Online Library Management System
An ASP.NET Core MVC web application designed to streamline library operations, including book rentals, returns, and user management.​

📝 Table of Contents
About the Project

Features

Tech Stack

Getting Started

Usage

Contributors

License

📖 About the Project
The Online Library Management System facilitates efficient management of library resources. It allows administrators to oversee book inventories, librarians to manage book rentals, and readers to borrow and return books seamlessly.​

✨ Features
User Authentication & Authorization

Role-Based Access Control (Admin, Librarian, Reader)

Book Catalog Management

Book Rental and Return Functionality

Late Fee and Fine Calculation

Responsive UI with Bootstrap

DataTables Integration for Enhanced Table Features​
YouTube
+7
Reddit
+7
Gist
+7

🛠️ Tech Stack
Frontend: HTML5, CSS3, JavaScript, Bootstrap 5, jQuery, DataTables

Backend: ASP.NET Core MVC, Entity Framework Core

Database: SQL Server

Version Control: Git & GitHub​
Wired
+1
FreeCodeCamp
+1

🚀 Getting Started
Prerequisites
Visual Studio 2019 or later

.NET 5.0 SDK or later

SQL Server​
GitHub Docs
+12
Wired
+12
Gist
+12

Installation
Clone the Repository

bash
Copy
Edit
git clone https://github.com/Jay-Dalsaniya/OnlineLibraryManagementSystem.git
Open the Project

Open the solution file OnlineLibraryManagementSystem.sln in Visual Studio.

Configure the Database

Update the appsettings.json file with your SQL Server connection string:

json
Copy
Edit
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=LibraryDB;Trusted_Connection=True;"
}
Apply Migrations

Open the Package Manager Console and run:

bash
Copy
Edit
Update-Database
Run the Application

Press F5 or click on the "Start" button in Visual Studio to run the application.

📷 Usage



👥 Contributors
Jay Dalsaniya

[Add other contributors here]​
en.wikipedia.org

📄 License
This project is licensed under the MIT License.
