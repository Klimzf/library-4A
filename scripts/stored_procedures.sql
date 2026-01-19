USE HomeLibrary;
GO

DROP PROCEDURE IF EXISTS InsertBook, GetBooks, GetBookById, UpdateBook, DeleteBook;
GO

CREATE PROCEDURE InsertBook
    @Title NVARCHAR(255),
    @Author NVARCHAR(255),
    @PublicationYear INT,
    @ContentXML XML
AS
BEGIN
    INSERT INTO Books (Title, Author, PublicationYear, ContentXML)
    VALUES (@Title, @Author, @PublicationYear, @ContentXML);
END;
GO

CREATE PROCEDURE GetBooks
AS
BEGIN
    SELECT * FROM Books;
END;
GO

CREATE PROCEDURE GetBookById
    @Id INT
AS
BEGIN
    SELECT * FROM Books WHERE Id = @Id;
END;
GO

CREATE PROCEDURE UpdateBook
    @Id INT,
    @Title NVARCHAR(255),
    @Author NVARCHAR(255),
    @PublicationYear INT,
    @ContentXML XML
AS
BEGIN
    UPDATE Books
    SET 
        Title = @Title,
        Author = @Author,
        PublicationYear = @PublicationYear,
        ContentXML = @ContentXML
    WHERE Id = @Id;
END;
GO

CREATE PROCEDURE DeleteBook
    @Id INT
AS
BEGIN
    DELETE FROM Books WHERE Id = @Id;
END;
GO