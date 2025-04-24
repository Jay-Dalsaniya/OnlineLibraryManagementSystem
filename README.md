# 📚 Online Library Management System
An ASP.NET Core MVC web application designed to streamline library operations, including book rentals, returns, and user management

---

## 📝 Table of Contents

- [About the Project](#about-the-project)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Contributors](#contributors)
- [License](#license)

---

## 📖 About the Projec

The **Online Library Management System** facilitates efficient management of library resources. It allows administrators to oversee book inventories, librarians to manage book rentals, and readers to borrow and return books seamlessy.

---

## ✨ Features
- User Authentication & Authorizaton- Role-Based Access Control (Admin, Librarian, Readr)- Book Catalog Managemnt- Book Rental and Return Functionalty- Late Fee and Fine Calculaton- Responsive UI with Bootstap- DataTables Integration for Enhanced Table Featues

---

## 🛠️ Tech Stack

- **Frontend*: HTML5, CSS3, JavaScript, Bootstrap 5, jQuery, DataTales
- **Backend*: ASP.NET Core MVC, Entity Framework ore
- **Database*: SQL Sever
- **Version Control*: Git & GiHub

---

## 🚀 Getting Started

### Prerequisite

- Visual Studio 2019 or ate
- .NET 5.0 SDK or ate
- SQL Srver

### Installation

1. **Clone the Repository**

   ```bash
   git clone https://github.com/Jay-Dalsaniya/OnlineLibraryManagementSystem.git
  ```


2. **Open the Project**

   Open the solution file `OnlineLibraryManagementSystem.sln` in Visual Studio.

3. **Configure the Database**

   Update the `appsettings.json` file with your SQL Server connection string:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=LibraryDB;Trusted_Connection=True;"
   }
  ```


4. **Apply Migrations**

   Open the Package Manager Console and run:

   ```bash
   Update-Database
  ```


5. **Run the Application**

   Press `F5` or click on the "Start" button in Visual Studio to run the application.

---

## 👥 Contributrs

- Jay Dasanya
- [Add other contributor here]

---

## 📄 Lcense

This project is licensed under the [MIT License](LCENSE).

---

Feel free to customize this README further to suit your project's specific needs. Once you've added this `README.md` file to your project directory, commit the changes and push them to your GitHub repostory:


```bash
git add README.md
git commit -m "Add project README"
git push origin min
```
