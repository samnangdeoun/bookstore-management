using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BookstoreManagement.Data
{
    public static class DatabaseInitializer
    {
        public static void EnsureDatabaseExists()
        {
            try
            {
                string dbFile = Path.Combine(Application.StartupPath, "BookstoreDB.mdf");
                Console.WriteLine($"Database file path: {dbFile}");
                if (!File.Exists(dbFile))
                {
                    Console.WriteLine("Database file not found. Creating a new database.");
                    CreateDatabase(dbFile);
                    CreateTables();
                    SeedData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing database: {ex.Message}", "Database Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        PasswordHash NVARCHAR(256),
                        Role NVARCHAR(50)
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
                    CREATE TABLE Discounts (
                        Id INT PRIMARY KEY IDENTITY,
                        BookId INT FOREIGN KEY REFERENCES Books(Id),
                        DiscountPercent INT,
                        StartDate DATE,
                        EndDate DATE
                    );
                    CREATE TABLE Reservations (
                        Id INT PRIMARY KEY IDENTITY,
                        BookId INT FOREIGN KEY REFERENCES Books(Id),
                        BuyerName NVARCHAR(100),
                        ReserveDate DATE
                    );
                    CREATE TABLE Sales (
                        Id INT PRIMARY KEY IDENTITY,
                        BookId INT FOREIGN KEY REFERENCES Books(Id),
                        SaleDate DATE,
                        SalePrice DECIMAL(10, 2)
                    );
                ";
                cmd.ExecuteNonQuery();
            }
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private static void SeedData()
        {
            using (var db = new BookstoreDataDataContext())
            using (var transaction = db.Connection.BeginTransaction())
            {
                try
                {
                    if (!db.Users.Any())
                    {
                        db.Users.InsertOnSubmit(new User
                        {
                            Username = "admin",
                            PasswordHash = HashPassword("admin123"),
                            Role = "Administrator"
                        });
                    }

                    if (!db.Genres.Any())
                    {
                        var genres = new[]
                        {
                            new Genre { Name = "Fiction" },
                            new Genre { Name = "Non-Fiction" },
                            new Genre { Name = "Science Fiction" },
                            new Genre { Name = "Mystery" },
                            new Genre { Name = "Romance" },
                            new Genre { Name = "Fantasy" },
                            new Genre { Name = "Biography" },
                            new Genre { Name = "History" },
                            new Genre { Name = "Children's" },
                            new Genre { Name = "Young Adult" }
                        };
                        db.Genres.InsertAllOnSubmit(genres);
                        db.SubmitChanges();

                        // Add sample books
                        var fiction = db.Genres.First(g => g.Name == "Fiction");
                        var sciFi = db.Genres.First(g => g.Name == "Science Fiction");

                        var books = new[]
                        {
                            new Book
                            {
                                Name = "The Great Novel",
                                AuthorFullName = "John Smith",
                                PublishingHouse = "Classic Books",
                                Pages = 320,
                                GenreId = fiction.Id,
                                DatePublished = DateTime.Parse("2023-01-15"),
                                PrimeCost = 15.99M,
                                SalePrice = 29.99M,
                                IsSequel = false,
                            },
                            new Book
                            {
                                Name = "Space Adventures",
                                AuthorFullName = "Sarah Johnson",
                                PublishingHouse = "Future Press",
                                Pages= 450,
                                GenreId = sciFi.Id,
                                DatePublished = DateTime.Parse("2023-03-20"),
                                PrimeCost = 18.99M,
                                SalePrice = 34.99M,
                                IsSequel = false,
                            }
                        };

                        db.Books.InsertAllOnSubmit(books);
                    }

                    db.SubmitChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
