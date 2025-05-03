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
    PublisherName NVARCHAR(200),
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
