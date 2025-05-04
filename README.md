# Bookstore Management System

A Windows Forms application for managing a bookstore, including books, genres, users, reservations, sales, and reports. This project uses **Entity Framework (LINQ to SQL)** for data operations and **ADO.NET** for backend management.

## Features

- User authentication with hashed passwords (Admin Role).
- Genre and book management.
- Auto-seeding of initial data for users, genres, and books.
- Data clearing utility to reset the database.
- Reporting functionalities (e.g., new releases, search results).
- Easy-to-extend structure for additional features like sales and discounts.

---

## Project Structure

- **Models/** – Contains entity definitions for `Book`, `Genre`, `Users`, `Sale`, etc.
- **Data/** – Contains `BookStoreContext` and `DatabaseInitializer` to manage and seed the database.
- **Forms/** – Windows Forms for user interaction and report generation.
- **ReportsForm.cs** – Shows categorized reports like "New Releases" and "Search Results".

---

## Database Initialization

The `DatabaseInitializer` class handles:

- **Database creation:** Automatically creates the database if it does not exist.
- **Data seeding:** Adds default users (`admin`), predefined genres, and example books.
- **Clear/reset functionality:** Commented out by default, the `clearDatabaseInformation()` method removes all records while respecting foreign key constraints.

### To Seed the Database:

```csharp
DatabaseInitializer.EnsureDatabaseExists();
