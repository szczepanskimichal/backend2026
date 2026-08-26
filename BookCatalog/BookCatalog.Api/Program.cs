
using BookCatalog.Api.Data;
using BookCatalog.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<
    IBookRepository, 
    //FileBookRepository>(); // this is
    SqlBookRepository>();

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

app.MapPost(
    "/books",
    async (
        Book book,
        IBookRepository repository) =>
    {
        await repository.AddAsync(book);

        return Results.Created(
            $"/books/{book.Id}",
            book);
    });

app.MapPut(
    "/books/{id:int}",
    async (
        int id,
        Book book,
        IBookRepository repository) =>
    {
        book.Id = id;

        var updated =
            await repository.UpdateAsync(book);

        if (!updated)
        {
            return Results.NotFound();
        }

        return Results.NoContent();
    });

app.MapDelete(
    "/books/{id:int}",
    async (
        int id,
        IBookRepository repository) =>
    {
        var deleted =
            await repository.DeleteAsync(id);

        if (!deleted)
        {
            return Results.NotFound();
        }

        return Results.NoContent();
    });
app.Run();