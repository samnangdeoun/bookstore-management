using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace BookstoreManagement.Data
{
    public static class DatabaseInitializer
    {
        public static void EnsureDatabaseExists()
        {
            try
            {
                string dbFile = Path.Combine(Application.StartupPath, "Bookstore.mdf");
                Console.WriteLine($"Database file path: {dbFile}");
                if (!File.Exists(dbFile))
                {
                    Console.WriteLine("Database file not found. Creating a new database.");
                    CreateDatabase(dbFile);
                    CreateTables();
                    SeedData();
                }
                else
                {
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
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\BookStore.mdf;Integrated Security=True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    CREATE TABLE Genre (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Name NVARCHAR(100) NOT NULL
                    );

                    CREATE TABLE Book (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Name NVARCHAR(200) NOT NULL,
                        AuthorName NVARCHAR(150),
                        Pages INT,
                        GenreId INT,
                        DatePublished DATE,
                        PrimeCost DECIMAL(10,2),
                        SalePrice DECIMAL(10,2),
                        IsSequel BIT,
                        FOREIGN KEY (GenreId) REFERENCES Genre(Id)
                    );

                    CREATE TABLE Sale (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        BookId INT NOT NULL,
                        Quantity INT NOT NULL,
                        TotalPrice DECIMAL(10,2),
                        DateSold DATETIME,
                        FOREIGN KEY (BookId) REFERENCES Book(Id)
                    );

                    CREATE TABLE Reservation (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        BookId INT NOT NULL,
                        CustomerName NVARCHAR(150),
                        ReservedAt DATETIME,
                        FOREIGN KEY (BookId) REFERENCES Book(Id)
                    );

                    CREATE TABLE Discount (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        GenreId INT NOT NULL,
                        Percentage DECIMAL(5,2),
                        StartDate DATE,
                        EndDate DATE,
                        FOREIGN KEY (GenreId) REFERENCES Genre(Id)
                    );

                    CREATE TABLE Users (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Username NVARCHAR(100) NOT NULL,
                        PasswordHash NVARCHAR(256) NOT NULL,
                        Role NVARCHAR(50) NOT NULL
                    );

                    CREATE TABLE BookStat (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        BookId INT NOT NULL,
                        SoldCount INT,
                        LastSoldAt DATETIME,
                        FOREIGN KEY (BookId) REFERENCES Book(Id)
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
            using (var db = new BookStoreDBDataContext())
            {
                // ✅ Ensure connection is open before using transaction
                if (db.Connection.State != System.Data.ConnectionState.Open)
                {
                    db.Connection.Open();
                }

                using (var transaction = db.Connection.BeginTransaction())
                {
                    db.Transaction = transaction;

                    try
                    {
                        if (!db.Users.Any())
                        {
                            db.Users.InsertOnSubmit(new User
                            {
                                Id = 1,
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

                            var fiction = db.Genres.First(g => g.Name == "Fiction");
                            var sciFi = db.Genres.First(g => g.Name == "Science Fiction");

                            var books = new[]
                            {
                                new Book
                                {
                                    Name = "The Great Novel",
                                    AuthorName = "John Smith",
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
                                    AuthorName = "Sarah Johnson",
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
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Error seeding database: {ex.Message}", "Seed Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw;
                    }
                }
            }
        }
    }
}
