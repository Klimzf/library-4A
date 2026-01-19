using HomeLibraryApp.Data;
using HomeLibraryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using System.Text;

namespace HomeLibraryApp.Controllers;

public class BooksController(AppDbContext context) : Controller
{
    public IActionResult Index()
    {
        var books = context.Books
            .FromSqlRaw("EXEC dbo.GetBooks")
            .ToList();
        return View(books);
    }

    public IActionResult Details(int id)
    {
        var book = context.Books
            .FromSqlRaw("EXEC dbo.GetBookById @Id", new SqlParameter("@Id", id))
            .AsEnumerable() 
            .FirstOrDefault();
        
        if (book == null)
            return NotFound();
            
        return View(book);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(Book book)
    {
        context.Database.ExecuteSqlRaw(
            "EXEC dbo.InsertBook @Title, @Author, @PublicationYear, @ContentXML",
            new SqlParameter("@Title", book.Title),
            new SqlParameter("@Author", book.Author),
            new SqlParameter("@PublicationYear", book.PublicationYear),
            new SqlParameter("@ContentXML", book.ContentXML)
        );
        return RedirectToAction("Index");
    }

    public IActionResult EditContent(int id)
    {
        var book = context.Books
            .FromSqlRaw("EXEC dbo.GetBookById @Id", new SqlParameter("@Id", id))
            .AsEnumerable()
            .FirstOrDefault();
        
        if (book == null)
            return NotFound();
            
        return View(book);
    }

    [HttpPost]

    public IActionResult EditContent(int id, [FromBody] XmlUpdateModel model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.Content))
        {
            return Json(new { success = false, message = "Пустой XML" });
        }

        try
        {
            using var stringReader = new StringReader(model.Content);
            using var xmlReader = XmlReader.Create(stringReader, new XmlReaderSettings { 
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null
            });
            
            var doc = new XmlDocument();
            doc.Load(xmlReader);
            
            context.Database.ExecuteSqlRaw(
                "UPDATE Books SET ContentXML = @ContentXML WHERE Id = @Id",
                new SqlParameter("@ContentXML", model.Content),
                new SqlParameter("@Id", id)
            );
            
            return Json(new { success = true, message = "XML успешно обновлен" });
        }
        catch (XmlException ex)
        {
            var errorMessage = $"Ошибка XML (строка {ex.LineNumber}, позиция {ex.LinePosition}): {ex.Message}";
            return Json(new { success = false, message = errorMessage });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Серверная ошибка: {ex.Message}" });
        }
    }

    public IActionResult Delete(int id)
    {
        context.Database.ExecuteSqlRaw(
            "EXEC dbo.DeleteBook @Id", 
            new SqlParameter("@Id", id)
        );
        return RedirectToAction("Index");
    }
}

public class XmlUpdateModel
{
    public string Content { get; set; } = string.Empty;
}