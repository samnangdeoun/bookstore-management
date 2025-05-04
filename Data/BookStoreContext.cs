using BookstoreManagement.Models;
using System.Data.Entity;

namespace BookstoreManagement.Data
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext() : base("name=BookStoreConnectionContext") { }

        public DbSet<BookStat> BookStats { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationItem> ReservationItems { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .Property(b => b.PrimeCost)
                .HasColumnType("decimal")
                .HasPrecision(18, 2);

            modelBuilder.Entity<Book>()
                .Property(b => b.PrimeCost)
                .HasColumnType("decimal")
                .HasPrecision(18, 2);

            modelBuilder.Entity<Sale>().Property(s => s.TotalPrice)
                .HasColumnType("decimal")
                .HasPrecision(10, 2);

            modelBuilder.Entity<ReservationItem>().Property(s => s.TotalPrice).HasColumnType("decimal").HasPrecision(10, 2);

            modelBuilder.Entity<ReservationItem>().Property(s => s.Discount).HasColumnType("decimal").HasPrecision(10, 2);

            modelBuilder.Entity<ReservationItem>().Property(s => s.UnitPrice).HasColumnType("decimal").HasPrecision(10, 2);


        }
    }
}
