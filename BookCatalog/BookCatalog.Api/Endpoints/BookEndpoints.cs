using Application.Abstractions;
using Domain.Entities;

namespace BookCatalog.Api.Endpoints;

public static class BookEndpoints
{
    public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/books");

        group.MapGet(string.Empty, async (string? author, IBookRepository repository) =>
        {
            if (!string.IsNullOrWhiteSpace(author))
            {
                var books = await repository.FindByAuthorAsync(author);
                return Results.Ok(books);
            }

            var allBooks = await repository.GetAllBooksAsync();
            return Results.Ok(allBooks);
        });

        group.MapGet("/{id:int}", async (int id, IBookRepository repository) =>
        {
            var book = await repository.FindAsync(id);
            return book is null ? Results.NotFound() : Results.Ok(book);
        });

        group.MapPost(string.Empty, async (Book book, IBookRepository repository) =>
        {
            await repository.AddAsync(book);
            return Results.Created($"/books/{book.Id}", book);
        });

        group.MapPut("/{id:int}", async (int id, Book book, IBookRepository repository) =>
        {
            book.Id = id;
            var updated = await repository.UpdateAsync(book);

            return updated ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id:int}", async (int id, IBookRepository repository) =>
        {
            var deleted = await repository.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });

        return app;
    }
}
