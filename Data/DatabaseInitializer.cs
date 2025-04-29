using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace BookstoreManagement.Data
{
    public static class DatabaseInitializer
    {
        public static void EnsureDatabaseExists()
        {
            string dbFile = Path.Combine(Application.StartupPath, "BookstoreDB.mdf");
            if (!File.Exists(dbFile))
            {
                CreateDatabase(dbFile);
                CreateTables();
                SeedData();
            }
        }

        private static void CreateDatabase(string dbFile)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = $"CREATE DATABASE [BookstoreDB] ON (NAME = N'BookstoreDB', FILENAME = '{dbFile}')";
                cmd.ExecuteNonQuery();
            }
            SqlConnection.ClearAllPools();
        }

        private static void CreateTables()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\BookstoreDB.mdf;Integrated Security=True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    CREATE TABLE Users (
                        Id INT PRIMARY KEY IDENTITY,
                        Username NVARCHAR(50) UNIQUE,
                        PasswordHash NVARCHAR(256)
                    );
                    CREATE TABLE Genres (
                        Id INT PRIMARY KEY IDENTITY,
                        Name NVARCHAR(50) UNIQUE
                    );
                    CREATE TABLE Books (
                        Id INT PRIMARY KEY IDENTITY,
                        Name NVARCHAR(100),
                        AuthorFullName NVARCHAR(100),
                        PublishingHouse NVARCHAR(100),
                        PageCount INT,
                        GenreId INT FOREIGN KEY REFERENCES Genres(Id),
                        PublishDate DATE,
                        PrimeCost DECIMAL(10, 2),
                        SalePrice DECIMAL(10, 2),
                        IsSequel BIT,
                        IsSold BIT DEFAULT 0
                    );
                    CREATE TABLE BookSales (
                        Id INT PRIMARY KEY IDENTITY,
                        BookId INT FOREIGN KEY REFERENCES Books(Id),
                        DiscountPercent INT,
                        StartDate DATE,
                        EndDate DATE
                    );
                    CREATE TABLE SavedBooks (
                        Id INT PRIMARY KEY IDENTITY,
                        BookId INT FOREIGN KEY REFERENCES Books(Id),
                        BuyerName NVARCHAR(100),
                        ReserveDate DATE
                    );
                    CREATE TABLE BookSalesLog (
                        Id INT PRIMARY KEY IDENTITY,
                        BookId INT,
                        SaleDate DATE
                    );
                ";
                cmd.ExecuteNonQuery();
            }
        }

        private static void SeedData()
        {
            using (var db = new BookstoreDataContext())
            {
                if (!db.Users.Any())
                {
                    db.Users.InsertOnSubmit(new User
                    {
                        Username = "admin",
                        PasswordHash = "admin" // Replace with hashed password in production
                    });
                }

                if (!db.Genres.Any())
                {
                    db.Genres.InsertAllOnSubmit(new[]
                    {
                        new Genre { Name = "Fiction" },
                        new Genre { Name = "Non-Fiction" },
                        new Genre { Name = "Sci-Fi" },
                        new Genre { Name = "Mystery" },
                    });
                }

                db.SubmitChanges();
            }
        }
    }
}
