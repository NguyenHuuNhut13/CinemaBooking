-- Tạo cơ sở dữ liệu mới cho hệ thống đặt vé xem phim
CREATE DATABASE CinemaBookingDB;
GO
-- Sử dụng cơ sở dữ liệu vừa tạo
USE CinemaBookingDB;
GO
-- Tạo bảng Users (Người dùng)
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    FullName VARCHAR(255) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(20),
    Role VARCHAR(20) DEFAULT 'User',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);
GO
-- Tạo bảng Movies (Phim)
CREATE TABLE Movies (
    MovieID INT PRIMARY KEY IDENTITY(1,1),
    Title VARCHAR(255) NOT NULL,
    Genre VARCHAR(100),
    Director VARCHAR(255),
    Duration INT,
    Description TEXT,
    Rating VARCHAR(10),
    ReleaseDate DATE,
    Poster VARCHAR(255),
    TrailerURL VARCHAR(255)
);
GO
-- Tạo bảng Cinemas (Rạp chiếu)
CREATE TABLE Cinemas (
    CinemaID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(255) NOT NULL,
    Location VARCHAR(255),
    PhoneNumber VARCHAR(20),
    City VARCHAR(100)
);
GO
-- Tạo bảng Auditoriums (Phòng chiếu)
CREATE TABLE Auditoriums (
    AuditoriumID INT PRIMARY KEY IDENTITY(1,1),
    CinemaID INT,
    Name VARCHAR(100),
    SeatCapacity INT,
    FOREIGN KEY (CinemaID) REFERENCES Cinemas(CinemaID)
);
GO
-- Tạo bảng Seats (Ghế ngồi)
CREATE TABLE Seats (
    SeatID INT PRIMARY KEY IDENTITY(1,1),
    AuditoriumID INT,
	IsOccupied BIT,
    Row CHAR(1),
    Number INT,
    FOREIGN KEY (AuditoriumID) REFERENCES Auditoriums(AuditoriumID)
);

GO
-- Tạo bảng Showtimes (Suất chiếu)
CREATE TABLE Showtimes (
    ShowtimeID INT PRIMARY KEY IDENTITY(1,1),
    MovieID INT,
    AuditoriumID INT,
    ShowDate DATE,
    ShowTime TIME,
    FOREIGN KEY (MovieID) REFERENCES Movies(MovieID),
    FOREIGN KEY (AuditoriumID) REFERENCES Auditoriums(AuditoriumID)
);
GO
-- Tạo bảng Tickets (Vé)
CREATE TABLE Tickets (
    TicketID INT PRIMARY KEY IDENTITY(1,1),
    ShowtimeID INT,
    SeatID INT,
    UserID INT,
    Price DECIMAL(10, 2),
    BookingTime DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ShowtimeID) REFERENCES Showtimes(ShowtimeID),
    FOREIGN KEY (SeatID) REFERENCES Seats(SeatID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
GO
-- Tạo bảng Payments (Thanh toán)
CREATE TABLE Payments (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    TicketID INT,
    PaymentMethod VARCHAR(50),
    Amount DECIMAL(10, 2),
    PaymentDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (TicketID) REFERENCES Tickets(TicketID)
);
GO
-- Tạo bảng Promotions (Khuyến mãi)
CREATE TABLE Promotions (
    PromotionID INT PRIMARY KEY IDENTITY(1,1),
    Code VARCHAR(20) NOT NULL,
    Discount DECIMAL(5, 2),
    StartDate DATE,
    EndDate DATE
);
GO
-- Tạo bảng Reviews (Đánh giá phim)
CREATE TABLE Reviews (
    ReviewID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT,
    MovieID INT,
    Rating INT CHECK (Rating BETWEEN 1 AND 10),
    Comment TEXT,
    ReviewDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (MovieID) REFERENCES Movies(MovieID)
);
GO
-- Tạo dữ liệu mẫu cho bảng Users
INSERT INTO Users (FullName, Email, PasswordHash, PhoneNumber, Role) VALUES
(N'Nguyễn Văn An', N'nguyenvana@gmail.com', N'hashedpassword123', N'0901234567', N'User'),
(N'Trần Thị Bích', N'tranthibich@gmail.com', N'hashedpassword456', N'0902345678', N'User'),
(N'Quản trị viên', N'admin@example.com', N'adminpasswordhash', N'0903456789', N'Admin');

GO

-- Tạo dữ liệu mẫu cho bảng Movies
INSERT INTO Movies (Title, Genre, Director, Duration, Description, Rating, ReleaseDate, Poster, TrailerURL) VALUES
(N'Avengers: Hồi Kết', N'Hành động', N'Anthony Russo', 180, N'Cuộc đối đầu cuối cùng giữa các siêu anh hùng và Thanos.', N'PG-13', '2019-04-26', N'avengers.jpg', N'https://youtube.com/trailer1'),
(N'Kẻ Đánh Cắp Giấc Mơ', N'Khoa học viễn tưởng', N'Christopher Nolan', 148, N'Một bộ phim kinh dị với những tình tiết gây xoắn não.', N'PG-13', '2010-07-16', N'inception.jpg', N'https://youtube.com/trailer2'),
(N'Titanic', N'Tình cảm', N'James Cameron', 195, N'Một câu chuyện tình kinh điển trên con tàu Titanic.', N'PG-13', '1997-12-19', N'titanic.jpg', N'https://youtube.com/trailer_titanic'),
(N'Người Sắt', N'Hành động', N'Jon Favreau', 126, N'Sự ra đời của Người Sắt trong vũ trụ Marvel.', N'PG-13', '2008-05-02', N'ironman.jpg', N'https://youtube.com/trailer_ironman'),
(N'Harry Potter và Hòn Đá Phù Thủy', N'Phiêu lưu', N'Chris Columbus', 152, N'Hành trình đầu tiên của Harry Potter tại Hogwarts.', N'PG', '2001-11-16', N'harrypotter.jpg', N'https://youtube.com/trailer_harrypotter'),
(N'Avatar', N'Viễn tưởng', N'James Cameron', 162, N'Một cuộc phiêu lưu đầy màu sắc trên hành tinh Pandora.', N'PG-13', '2009-12-18', N'avatar.jpg', N'https://youtube.com/trailer_avatar'),
(N'Fast & Furious 9', N'Hành động', N'Justin Lin', 143, N'Tiếp tục hành trình tốc độ và cảm xúc.', N'PG-13', '2021-05-19', N'fast9.jpg', N'https://youtube.com/trailer_fast9');
GO

-- Tạo dữ liệu mẫu cho bảng Cinemas
INSERT INTO Cinemas (Name, Location, PhoneNumber, City) VALUES
(N'CinemaWorld Vincom', N'72A Nguyễn Trãi, Hà Nội', N'0241234567', N'Hà Nội'),
(N'CinemaWorld Aeon Mall', N'12 Tôn Thất Tùng, TP Hồ Chí Minh', N'0281234567', N'TP Hồ Chí Minh'),
(N'CinemaWorld Phan Thiết', N'06 Lê Hồng Phong, Phan Thiết, Bình Thuận', N'02523874567', N'Phan Thiết'),
(N'CinemaWorld Nguyễn Văn Cừ', N'116 Nguyễn Văn Cừ, Quận 5, TP Hồ Chí Minh', N'0285678913', N'TP Hồ Chí Minh'),
(N'CinemaWorld Phạm Ngọc Thạch', N'Tầng 4, 8 Phạm Ngọc Thạch, Quận 3, TP Hồ Chí Minh', N'0285678914', N'TP Hồ Chí Minh'),
(N'CinemaWorld Cao Thắng', N'Tầng 6, 19 Cao Thắng, Quận 3, TP Hồ Chí Minh', N'0285678915', N'TP Hồ Chí Minh'),
(N'CinemaWorld Crescent Mall', N'Tầng 5, 101 Tôn Dật Tiên, Quận 7, TP Hồ Chí Minh', N'0285678916', N'TP Hồ Chí Minh');
GO

-- Tạo dữ liệu mẫu cho bảng Auditoriums
INSERT INTO Auditoriums (CinemaID, Name, SeatCapacity) VALUES
(1, N'Phòng chiếu 1', 100),
(1, N'Phòng chiếu 2', 120),
(2, N'Phòng chiếu 1', 150),
(3, N'Phòng chiếu 1', 120),
(3, N'Phòng chiếu 2', 150),
(4, N'Phòng chiếu 1', 100),
(4, N'Phòng chiếu 2', 140),
(5, N'Phòng chiếu 1', 180),
(5, N'Phòng chiếu 2', 200);
GO

-- Tạo dữ liệu mẫu cho bảng Seats
INSERT INTO Seats (AuditoriumID,IsOccupied, Row, Number) VALUES
(1,0, N'A', 1), (1,0, N'A', 2), (1,1, N'A', 3),
(2,0, N'B', 1), (2,0, N'B', 2), (2,0, N'B', 3),
(3,1, N'C', 1), (3,0, N'C', 2), (3,1, N'C', 3),
(4, 0, N'D', 1), (4, 0, N'D', 2), (4, 1, N'D', 3),
(4, 0, N'E', 1), (4, 1, N'E', 2), (4, 0, N'E', 3),
(5, 0, N'F', 1), (5, 0, N'F', 2), (5, 1, N'F', 3),
(5, 1, N'G', 1), (5, 0, N'G', 2), (5, 0, N'G', 3),
(6, 1, N'H', 1), (6, 0, N'H', 2), (8, 0, N'A', 1), (8, 0, N'A', 2), (8, 0, N'A', 3),
(8, 1, N'B', 1), (8, 1, N'B', 2), (8, 0, N'B', 3),
(9, 1, N'C', 1), (9, 0, N'C', 2), (9, 1, N'C', 3), 
(9, 0, N'D', 1), (9, 0, N'D', 2);

GO

-- Tạo dữ liệu mẫu cho bảng Showtimes
INSERT INTO Showtimes (MovieID, AuditoriumID, ShowDate, ShowTime) VALUES
(1, 1, '2024-10-10', '18:00:00'),
(2, 2, '2024-10-11', '20:00:00'),
(3, 4, '2024-10-15', '17:30:00'),
(4, 5, '2024-10-16', '19:00:00'),
(5, 6, '2024-10-17', '20:30:00'),
(1, 7, '2024-10-18', '18:00:00'),
(2, 8, '2024-10-19', '21:00:00');
GO

-- Tạo dữ liệu mẫu cho bảng Tickets
INSERT INTO Tickets (ShowtimeID, SeatID, UserID, Price) VALUES
(1, 1, 1, 100000),
(1, 2, 2, 100000),
(2, 3, 1, 120000),
(3, 1, 3, 110000),
(3, 2, 2, 110000),
(4, 3, 2, 130000);
GO


-- Tạo dữ liệu mẫu cho bảng Payments
INSERT INTO Payments (TicketID, PaymentMethod, Amount) VALUES
(2, N'Thẻ tín dụng', 100000),
(3, N'Chuyển khoản', 120000),
(4, N'Thẻ tín dụng', 110000),
(5, N'Tiền mặt', 110000),
(6, N'Chuyển khoản', 130000);
GO

-- Tạo dữ liệu mẫu cho bảng Promotions
INSERT INTO Promotions (Code, Discount, StartDate, EndDate) VALUES
(N'NEWYEAR2024', 10.00, '2024-01-01', '2024-01-31'),
(N'SUMMER2024', 15.00, '2024-06-01', '2024-06-30'),
(N'VIPMEMBER', 20.00, '2024-03-01', '2024-03-31');
GO

-- Tạo dữ liệu mẫu cho bảng Reviews
INSERT INTO Reviews (UserID, MovieID, Rating, Comment) VALUES
(1, 1, 9, N'Phim rất hay, hiệu ứng đặc sắc!'),
(2, 3, 8, N'Một câu chuyện tình cảm xúc, nhưng kết thúc hơi buồn.'),
(3, 5, 10, N'Bộ phim phiêu lưu tuyệt vời, không thể bỏ lỡ!'),
(1, 6, 7, N'Phim đẹp mắt nhưng nội dung không quá xuất sắc.'),
(2, 2, 9, N'Một bộ phim hấp dẫn và đầy thử thách cho người xem.');
GO
SELECT * FROM Tickets WHERE TicketID IN (1, 2, 3, 4, 5, 6);
