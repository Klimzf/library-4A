CREATE DATABASE HomeLibrary;
GO

USE HomeLibrary;
GO

CREATE TABLE Books (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Author NVARCHAR(255) NOT NULL,
    PublicationYear INT,
    ContentXML XML
);
GO