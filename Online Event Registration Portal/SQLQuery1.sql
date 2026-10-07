CREATE DATABASE EventDB;
GO

USE EventDB;
GO

CREATE TABLE EventRegistrations (
    RegistrationId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Age INT NOT NULL,
    EventTrack NVARCHAR(100) NOT NULL,
    RegistrationRef NVARCHAR(50) NOT NULL UNIQUE,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO