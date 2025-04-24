INSERT INTO Books (Title, Author, ISBN, Price, PublishedDate, BookImage, Genre, Category, Subject, Condition, Summary, TotalCopies, Language, Edition, Publisher, CreatedBy, CreateDate, ModifyDate, ModifyBy, Id)
VALUES
('The Lost Kingdom', 'William Green', '9781234567890', 2872, '2019-03-25', 'the_lost_kingdom_cover.jpg', 'Fantasy', 'Novel', 'History', 'New', 'A tale of a forgotten kingdom, long lost to time.', 12, 'English', '1st', 'System', 'System', GETDATE(), GETDATE(), 'System', 9);


select * from Books

SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Books';

INSERT INTO Books 
(Title, Author, ISBN, Price, PublishedDate,  Genre, Category, Subject, Condition, Summary, TotalCopies, Language, Edition, Publisher, CreatedBy, CreateDate, ModifyDate, ModifyBy, ImagePath, AvailableCopies, DateAdded, IsAvailable, IsDeleted, Rating)
VALUES
('The Lost World', 'Alice Green', '9789876543210', 799.00, '2022-09-18', 'Adventure', 'Novel', 'Science Fiction', 'New', 'A thrilling adventure in a world of unknown creatures.', 8, 'English', '3rd', 'Fiction Press', 'JayKumar', GETDATE(), GETDATE(), 'JayKumar', '/images/books/b5.jpg', 8, GETDATE(), 1, 0, 4.7);



INSERT INTO Books 
(Title, Author, ISBN, Price, PublishedDate, Genre, Category, Subject, Condition, Summary, TotalCopies, Language, Edition, Publisher, CreatedBy, CreateDate, ModifyDate, ModifyBy, ImagePath, AvailableCopies, DateAdded, IsAvailable, IsDeleted, Rating)
VALUES
('The Lost World', 'Alice Green', '9789876543210', 799.00, '2022-09-18', 'Adventure', 'Novel', 'Science Fiction', 'New', 'A thrilling adventure in a world of unknown creatures.', 8, 'English', '3rd', 'Fiction Press', 'JayKumar', GETDATE(), GETDATE(), 'JayKumar', '/images/books/b5.jpg', 8, GETDATE(), 1, 0, 4.7),
('Mystery of the Deep', 'John Smith', '9781234567890', 599.00, '2021-07-01', 'Mystery', 'Novel', 'Thriller', 'Used', 'A thrilling mystery set in the deep ocean.', 5, 'English', '2nd', 'Oceanic Books', 'JayKumar', GETDATE(), GETDATE(), 'JayKumar', '/images/books/b6.jpg', 5, GETDATE(), 1, 0, 4.3),
('The Quantum Theory', 'Sarah Mitchell', '9782345678901', 899.00, '2020-05-15', 'Science', 'Textbook', 'Physics', 'New', 'A comprehensive guide to the quantum world.', 10, 'English', '1st', 'Quantum Publishers', 'JayKumar', GETDATE(), GETDATE(), 'JayKumar', '/images/books/b7.jpg', 10, GETDATE(), 1, 0, 4.8),
('Cooking with Love', 'Emily Rose', '9783456789012', 349.00, '2021-11-20', 'Culinary', 'Cookbook', 'Cooking', 'New', 'A beautiful cookbook filled with delicious recipes for all occasions.', 12, 'English', '1st', 'Gourmet Press', 'JayKumar', GETDATE(), GETDATE(), 'JayKumar', '/images/books/b8.jpg', 12, GETDATE(), 1, 0, 4.5),
('Exploring the Universe', 'James Clark', '9784567890123', 1299.00, '2019-02-28', 'Science', 'Textbook', 'Astronomy', 'Used', 'An introduction to the vast wonders of the universe.', 6, 'English', 'Revised', 'Stellar Publications', 'JayKumar', GETDATE(), GETDATE(), 'JayKumar', '/images/books/b9.jpg', 6, GETDATE(), 1, 0, 4.6),
('The Art of Leadership', 'Linda White', '9785678901234', 799.00, '2020-08-10', 'Business', 'Self-help', 'Leadership', 'New', 'A guide to becoming an effective and inspirational leader.', 15, 'English', '2nd', 'Leadership Publishers', 'JayKumar', GETDATE(), GETDATE(), 'JayKumar', '/images/books/b10.jpg', 15, GETDATE(), 1, 0, 4.7),
('The Journey Within', 'Michael Brown', '9786789012345', 399.00, '2021-03-25', 'Spiritual', 'Novel', 'Self-discovery', 'New', 'A personal journey into the depths of the soul and mind.', 8, 'English', '1st', 'Mindful Press', 'JayKumar', GETDATE(), GETDATE(), 'JayKumar', '/images/books/b11.jpg', 8, GETDATE(), 1, 0, 4.4),
('History of Ancient Civilizations', 'Robert Jones', '9787890123456', 1299.00, '2018-10-15', 'History', 'Textbook', 'Ancient Civilizations', 'Used', 'A detailed exploration of the ancient civilizations that shaped history.', 5, 'English', 'Revised', 'History Books Ltd.', 'JayKumar', GETDATE(), GETDATE(), 'JayKumar', '/images/books/b12.jpg', 5, GETDATE(), 1, 0, 4.6),
('Innovation and Technology', 'David Lee', '9788901234567', 1199.00, '2022-01-10', 'Technology', 'Textbook', 'Innovation', 'New', 'A look at the cutting-edge technology transforming our world.', 10, 'English', '1st', 'Tech Press', 'JayKumar', GETDATE(), GETDATE(), 'JayKumar', '/images/books/b13.jpg', 10, GETDATE(), 1, 0, 4.9),
('The Secret of Happiness', 'Linda Green', '9789012345678', 549.00, '2017-12-05', 'Self-help', 'Novel', 'Personal Growth', 'New', 'A guide to finding happiness and fulfillment in life.', 10, 'English', '2nd', 'Happiness Press', 'JayKumar', GETDATE(), GETDATE(), 'JayKumar', '/images/books/b14.jpg', 10, GETDATE(), 1, 0, 4.8);



INSERT INTO Books 
(Title, Author, ISBN, Price, PublishedDate, Genre, Category, Subject, Condition, Summary, TotalCopies, Language, Edition, Publisher, CreatedBy, CreateDate, ModifyDate, ModifyBy, ImagePath, AvailableCopies, DateAdded, IsAvailable, IsDeleted, Rating)
VALUES
('The Adventure Begins', 'Manan Kumar', '9781122334455', 399.00, '2023-01-10', 'Adventure', 'Novel', 'Action', 'New', 'An exciting tale of an unexpected adventure.', 10, 'English', '1st', 'Adventure Books', 'Manan', GETDATE(), GETDATE(), 'Manan', '/images/books/b15.jpg', 10, GETDATE(), 1, 0, 4.6),
('Beyond the Horizon', 'Asha Patel', '9782233445566', 499.00, '2022-11-21', 'Fantasy', 'Novel', 'Mythology', 'Used', 'A gripping story set beyond the horizon in a magical land.', 8, 'English', '2nd', 'Fantasy World', 'Manan', GETDATE(), GETDATE(), 'Manan', '/images/books/b16.jpg', 8, GETDATE(), 1, 0, 4.5),
('Deep Space Discovery', 'Neel Verma', '9783344556677', 899.00, '2021-05-15', 'Science Fiction', 'Textbook', 'Astronomy', 'New', 'A fascinating exploration of deep space and distant galaxies.', 12, 'English', '3rd', 'Space Press', 'Manan', GETDATE(), GETDATE(), 'Manan', '/images/books/b17.jpg', 12, GETDATE(), 1, 0, 4.7),
('The Power of Cooking', 'Rohit Sharma', '9784455667788', 349.00, '2020-03-30', 'Culinary', 'Cookbook', 'Cooking', 'New', 'A collection of delicious and easy-to-follow recipes.', 15, 'English', '1st', 'Kitchen Wonders', 'Manan', GETDATE(), GETDATE(), 'Manan', '/images/books/b18.jpg', 15, GETDATE(), 1, 0, 4.4),
('Exploring Ancient Egypt', 'Simran Kaur', '9785566778899', 1299.00, '2019-02-01', 'History', 'Textbook', 'Archaeology', 'Used', 'An in-depth exploration of ancient Egyptian civilization.', 10, 'English', 'Revised', 'History Press', 'Manan', GETDATE(), GETDATE(), 'Manan', '/images/books/b19.jpg', 10, GETDATE(), 1, 0, 4.6),
('Mastering Leadership', 'Siddharth Gupta', '9786677889900', 799.00, '2021-07-15', 'Business', 'Self-help', 'Leadership', 'New', 'A comprehensive guide to mastering leadership skills.', 20, 'English', '2nd', 'Leadership Books', 'Manan', GETDATE(), GETDATE(), 'Manan', '/images/books/b20.jpg', 20, GETDATE(), 1, 0, 4.8),
('The Power of Mindfulness', 'Aarav Verma', '9787788990011', 399.00, '2022-01-05', 'Self-help', 'Novel', 'Mindfulness', 'New', 'A guide to practicing mindfulness for personal growth.', 12, 'English', '1st', 'Mindful Press', 'Manan', GETDATE(), GETDATE(), 'Manan', '/images/books/b21.jpg', 12, GETDATE(), 1, 0, 4.5),
('Digital Transformation', 'Ravi Agarwal', '9788899001122', 999.00, '2021-09-10', 'Technology', 'Textbook', 'Business', 'New', 'Exploring the impact of digital transformation on business.', 18, 'English', 'Revised', 'Tech World', 'Manan', GETDATE(), GETDATE(), 'Manan', '/images/books/b22.jpg', 18, GETDATE(), 1, 0, 4.7),
('Psychology of Emotions', 'Priya Reddy', '9789900112233', 599.00, '2020-06-20', 'Psychology', 'Textbook', 'Emotions', 'Used', 'A deep dive into understanding human emotions.', 8, 'English', '1st', 'Mind Books', 'Manan', GETDATE(), GETDATE(), 'Manan', '/images/books/b23.jpg', 8, GETDATE(), 1, 0, 4.4),
('The Future of Technology', 'Karan Mehta', '9781000113344', 1299.00, '2023-03-05', 'Technology', 'Textbook', 'Innovation', 'New', 'A look at the future of technology and its impact on society.', 25, 'English', '1st', 'Tech Innovations', 'Manan', GETDATE(), GETDATE(), 'Manan', '/images/books/b24.jpg', 25, GETDATE(), 1, 0, 4.9);




USE [LibraryManagementSystem]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BookRentals](
	[BookRentalId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[BookId] [int] NOT NULL,
	[RentDate] [datetime2](7) NOT NULL,
	[ReturnDate] [datetime2](7) NULL,
	[DueDate] [datetime2](7) NOT NULL,
	[LateFee] [decimal](18, 2) NULL,
	[BookCondition] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[RentalStatus] [nvarchar](20) NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[Author] [nvarchar](100) NOT NULL,
	[Category] [nvarchar](max) NOT NULL,
	[Edition] [nvarchar](max) NOT NULL,
	[Genre] [nvarchar](max) NOT NULL,
	[ISBN] [nvarchar](13) NOT NULL,
	[Language] [nvarchar](max) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[PublishedDate] [datetime2](7) NOT NULL,
	[Subject] [nvarchar](max) NOT NULL,
	[Summary] [nvarchar](max) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[TotalCopies] [int] NOT NULL,
	[ReaderId] [int] NOT NULL,
	[Fine] [decimal](18, 2) NULL,
	[RentalFee] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_BookRentals] PRIMARY KEY CLUSTERED 
(
	[BookRentalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [DueDate]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT (N'') FOR [BookCondition]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [CreatedDate]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT (N'') FOR [RentalStatus]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT (N'') FOR [Author]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT (N'') FOR [Category]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT (N'') FOR [Edition]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT (N'') FOR [Genre]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT (N'') FOR [ISBN]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT (N'') FOR [Language]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT ((0.0)) FOR [Price]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [PublishedDate]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT (N'') FOR [Subject]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT (N'') FOR [Summary]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT (N'') FOR [Title]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT ((0)) FOR [TotalCopies]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT ((0)) FOR [ReaderId]
GO

ALTER TABLE [dbo].[BookRentals] ADD  DEFAULT ((0.0)) FOR [RentalFee]
GO

ALTER TABLE [dbo].[BookRentals]  WITH CHECK ADD  CONSTRAINT [FK_BookRentals_Books_BookId] FOREIGN KEY([BookId])
REFERENCES [dbo].[Books] ([BookId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BookRentals] CHECK CONSTRAINT [FK_BookRentals_Books_BookId]
GO

ALTER TABLE [dbo].[BookRentals]  WITH CHECK ADD  CONSTRAINT [FK_BookRentals_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BookRentals] CHECK CONSTRAINT [FK_BookRentals_Users_UserId]
GO





USE [LibraryManagementSystem]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BookPurchases](
	[BookPurchaseId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[BookId] [int] NOT NULL,
	[PurchaseDate] [datetime2](7) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Author] [nvarchar](100) NOT NULL,
	[ISBN] [nvarchar](13) NOT NULL,
	[Genre] [nvarchar](50) NOT NULL,
	[Category] [nvarchar](50) NOT NULL,
	[Subject] [nvarchar](50) NOT NULL,
	[Summary] [nvarchar](500) NOT NULL,
	[TotalCopies] [int] NOT NULL,
	[Language] [nvarchar](50) NOT NULL,
	[Edition] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_BookPurchases] PRIMARY KEY CLUSTERED 
(
	[BookPurchaseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BookPurchases]  WITH CHECK ADD  CONSTRAINT [FK_BookPurchases_Books_BookId] FOREIGN KEY([BookId])
REFERENCES [dbo].[Books] ([BookId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BookPurchases] CHECK CONSTRAINT [FK_BookPurchases_Books_BookId]
GO

ALTER TABLE [dbo].[BookPurchases]  WITH CHECK ADD  CONSTRAINT [FK_BookPurchases_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BookPurchases] CHECK CONSTRAINT [FK_BookPurchases_Users_UserId]
GO





USE [LibraryManagementSystem]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Books](
	[BookId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Author] [nvarchar](100) NOT NULL,
	[ISBN] [nvarchar](13) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[PublishedDate] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[ModifyBy] [nvarchar](max) NOT NULL,
	[ModifyDate] [datetime2](7) NOT NULL,
	[ImagePath] [nvarchar](255) NULL,
	[AvailableCopies] [int] NOT NULL,
	[DateAdded] [datetime2](7) NOT NULL,
	[Edition] [nvarchar](max) NOT NULL,
	[Genre] [nvarchar](max) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Language] [nvarchar](max) NOT NULL,
	[Publisher] [nvarchar](max) NOT NULL,
	[Rating] [float] NOT NULL,
	[TotalCopies] [int] NOT NULL,
	[Category] [nvarchar](max) NOT NULL,
	[Condition] [nvarchar](max) NOT NULL,
	[Subject] [nvarchar](max) NOT NULL,
	[Summary] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED 
(
	[BookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Books] ADD  DEFAULT ((0)) FOR [AvailableCopies]
GO

ALTER TABLE [dbo].[Books] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [DateAdded]
GO

ALTER TABLE [dbo].[Books] ADD  DEFAULT (N'') FOR [Edition]
GO

ALTER TABLE [dbo].[Books] ADD  DEFAULT (N'') FOR [Genre]
GO

ALTER TABLE [dbo].[Books] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Books] ADD  DEFAULT (N'') FOR [Language]
GO

ALTER TABLE [dbo].[Books] ADD  DEFAULT (N'') FOR [Publisher]
GO

ALTER TABLE [dbo].[Books] ADD  DEFAULT ((0.0000000000000000e+000)) FOR [Rating]
GO

ALTER TABLE [dbo].[Books] ADD  DEFAULT ((0)) FOR [TotalCopies]
GO

ALTER TABLE [dbo].[Books] ADD  DEFAULT (N'') FOR [Category]
GO

ALTER TABLE [dbo].[Books] ADD  DEFAULT (N'') FOR [Condition]
GO

ALTER TABLE [dbo].[Books] ADD  DEFAULT (N'') FOR [Subject]
GO

ALTER TABLE [dbo].[Books] ADD  DEFAULT (N'') FOR [Summary]
GO






USE [LibraryManagementSystem]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[ImageName] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Birthday] [datetime2](7) NOT NULL,
	[Gender] [nvarchar](max) NOT NULL,
	[PhoneNumber] [nvarchar](max) NOT NULL,
	[Role] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [Address]
GO

ALTER TABLE [dbo].[Users] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [Birthday]
GO

ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [Gender]
GO

ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [PhoneNumber]
GO

ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [Role]
GO



