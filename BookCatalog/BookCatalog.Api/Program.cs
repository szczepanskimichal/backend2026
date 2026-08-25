
using BookCatalog.Api.Data;
using BookCatalog.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<
    IBookRepository, 
    FileBookRepository>();

var app = builder.Build();

app.MapGet("/books", async (string? author,
    IBookRepository repository) =>
        {
            if(!string.IsNullOrWhiteSpace(author))
            {var books = await repository.FindByAuthorAsync(author);
                return Results.Ok(books);
            }

            var allBooks = await repository.GetAllBooksAsync();
            return Results.Ok(allBooks);
        });

app.MapGet("/books/{id:int}", async (int id, IBookRepository repository) =>
{
    var book = await repository.FindAsync(id);

    if (book == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(book);
});

app.Run();