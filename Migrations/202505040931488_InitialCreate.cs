namespace BookstoreManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AuthorName = c.String(),
                        Pages = c.Int(nullable: false),
                        GenreId = c.Int(nullable: false),
                        DatePublished = c.DateTime(nullable: false),
                        PublisherName = c.String(),
                        PrimeCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsSequel = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Genres", t => t.GenreId, cascadeDelete: true)
                .Index(t => t.GenreId);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BookStats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        SoldCount = c.Int(nullable: false),
                        LastSoldAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Discounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GenreId = c.Int(nullable: false),
                        Percentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false, storeType: "date"),
                        EndDate = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Genres", t => t.GenreId, cascadeDelete: true)
                .Index(t => t.GenreId);
            
            CreateTable(
                "dbo.ReservationItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReservationId = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 10, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 10, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 10, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Reservations", t => t.ReservationId, cascadeDelete: true)
                .Index(t => t.ReservationId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(),
                        ReservedAt = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(),
                        BookId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        TotalPrice = c.Decimal(nullable: false, precision: 10, scale: 2),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateSold = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        PasswordHash = c.String(),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sales", "BookId", "dbo.Books");
            DropForeignKey("dbo.ReservationItems", "ReservationId", "dbo.Reservations");
            DropForeignKey("dbo.ReservationItems", "BookId", "dbo.Books");
            DropForeignKey("dbo.Discounts", "GenreId", "dbo.Genres");
            DropForeignKey("dbo.BookStats", "BookId", "dbo.Books");
            DropForeignKey("dbo.Books", "GenreId", "dbo.Genres");
            DropIndex("dbo.Sales", new[] { "BookId" });
            DropIndex("dbo.ReservationItems", new[] { "BookId" });
            DropIndex("dbo.ReservationItems", new[] { "ReservationId" });
            DropIndex("dbo.Discounts", new[] { "GenreId" });
            DropIndex("dbo.BookStats", new[] { "BookId" });
            DropIndex("dbo.Books", new[] { "GenreId" });
            DropTable("dbo.Users");
            DropTable("dbo.Sales");
            DropTable("dbo.Reservations");
            DropTable("dbo.ReservationItems");
            DropTable("dbo.Discounts");
            DropTable("dbo.BookStats");
            DropTable("dbo.Genres");
            DropTable("dbo.Books");
        }
    }
}
