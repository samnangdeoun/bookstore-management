using BookstoreManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BookstoreManagement.Data
{
    public static class DatabaseInitializer
    {
        private static bool IsDatabaseEmpty(BookStoreContext db)
        {
            return !db.Users.Any() && !db.Genres.Any() && !db.Books.Any();
        }

        private static void clearDatabaseInformation()
        {
            using(BookStoreContext db = new BookStoreContext())
            {
                if (db.Database.Exists())
                {
                    // Clear tables in order to respect foreign key constraints
                    db.Sales.RemoveRange(db.Sales);
                    db.Discounts.RemoveRange(db.Discounts);
                    db.Reservations.RemoveRange(db.Reservations);
                    db.BookStats.RemoveRange(db.BookStats);
                    db.Books.RemoveRange(db.Books);
                    db.Genres.RemoveRange(db.Genres);
                    db.Users.RemoveRange(db.Users);
                    
                    // Save all changes
                    db.SaveChanges();
                }
            }
        }

        public static void EnsureDatabaseExists()
        {
            try
            {
                using (BookStoreContext db = new BookStoreContext())
                {
                    if (!db.Database.Exists())
                    {
                        db.Database.CreateIfNotExists();
                    }

                    db.Database.Initialize(force: false);
                    if (db.Database.Exists() && IsDatabaseEmpty(db))
                    {
                        // Add users one by one
                        var admin = new Users
                        {
                            Username = "admin",
                            PasswordHash = HashPassword("admin123"),
                            Role = "Admin"
                        };
                        db.Users.Add(admin);

                        var manager = new Users
                        {
                            Username = "manager",
                            PasswordHash = HashPassword("manager123"),
                            Role = "Manager"
                        };
                        db.Users.Add(manager);

                        db.SaveChanges();

                        // Add genres one by one
                        var genreNames = new[]
                        {
                        "Fiction", "Non-Fiction", "Science Fiction", "Mystery", "Romance",
                        "Fantasy", "Biography", "History", "Children's", "Young Adult"
                    };

                        foreach (var name in genreNames)
                        {
                            db.Genres.Add(new Genre { Name = name });
                        }

                        db.SaveChanges();

                        // Retrieve genre objects
                        var fiction = db.Genres.FirstOrDefault(g => g.Name == "Fiction");
                        var sciFi = db.Genres.FirstOrDefault(g => g.Name == "Science Fiction");

                        // Add books one by one
                        var book1 = new Book
                        {
                            Name = "The Great Novel",
                            AuthorName = "John Smith",
                            Pages = 320,
                            GenreId = fiction.Id,
                            PublisherName = "Fiction House",
                            DatePublished = DateTime.Parse("2023-01-15"),
                            PrimeCost = 15.99M,
                            SalePrice = 29.99M,
                            IsSequel = false
                        };
                        db.Books.Add(book1);

                        var book2 = new Book
                        {
                            Name = "Space Adventures",
                            AuthorName = "Sarah Johnson",
                            Pages = 450,
                            GenreId = sciFi.Id,
                            PublisherName = "Galactic Press",
                            DatePublished = DateTime.Parse("2023-03-20"),
                            PrimeCost = 18.99M,
                            SalePrice = 34.99M,
                            IsSequel = false
                        };
                        db.Books.Add(book2);

                        var book3 = new Book
                        {
                            Name = "Mystery of the Lost Treasure",
                            AuthorName = "Emily Davis",
                            Pages = 280,
                            GenreId = db.Genres.FirstOrDefault(g => g.Name == "Mystery").Id,
                            PublisherName = "Mystery Press",
                            DatePublished = DateTime.Parse("2023-05-10"),
                            PrimeCost = 12.99M,
                            SalePrice = 24.99M,
                            IsSequel = false
                        };
                        db.Books.Add(book3);

                        var book4 = new Book
                        {
                            Name = "Romantic Journey",
                            AuthorName = "Michael Brown",
                            Pages = 350,
                            GenreId = db.Genres.FirstOrDefault(g => g.Name == "Romance").Id,
                            PublisherName = "Love Stories Publishing",
                            DatePublished = DateTime.Parse("2023-07-25"),
                            PrimeCost = 14.99M,
                            SalePrice = 27.99M,
                            IsSequel = false
                        };
                        db.Books.Add(book4);

                        var book5 = new Book
                        {
                            Name = "Fantasy World",
                            AuthorName = "Jessica Wilson",
                            Pages = 400,
                            GenreId = db.Genres.FirstOrDefault(g => g.Name == "Fantasy").Id,
                            PublisherName = "Fantasy Press",
                            DatePublished = DateTime.Parse("2023-09-30"),
                            PrimeCost = 16.99M,
                            SalePrice = 31.99M,
                            IsSequel = false
                        };
                        db.Books.Add(book5);

                        db.SaveChanges();
                    }
                    //else
                    //{
                    //    clearDatabaseInformation();
                    //}
                }
            }
            catch (Exception err)
            {
                Console.WriteLine($"Error initializing database: {err.Message}");
                Console.WriteLine("An error occurred: " + err.Message);
                Console.WriteLine("Inner Exception: " + err.InnerException?.Message);
                Console.WriteLine(err.StackTrace);
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
    }
}
