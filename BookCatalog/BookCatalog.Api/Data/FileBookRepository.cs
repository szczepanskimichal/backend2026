using System.Text.Json;
using BookCatalog.Api.Models;

namespace BookCatalog.Api.Data;

public class FileBookRepository : IBookRepository
{
    private readonly string _filePath;

    public FileBookRepository(IWebHostEnvironment environment)
    {
        _filePath = Path.Combine(
            environment.ContentRootPath,
            "db",
            "books.json");
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        var books = await LoadBooksAsync();

        return books;
    }

    public async Task<Book?> FindAsync(int id)
    {
        var books = await LoadBooksAsync();

        return books.FirstOrDefault(book => book.Id == id);
    }

    public async Task<IEnumerable<Book>> FindByAuthorAsync(string author)
    {
        var books = await LoadBooksAsync();

        return books
            .Where(book =>
                book.Author.Equals(
                    author,
                    StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public Task AddAsync(Book book)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Book book)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    private async Task<List<Book>> LoadBooksAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Book>();
        }

        var json = await File.ReadAllTextAsync(_filePath);

        return JsonSerializer.Deserialize<List<Book>>(json)
               ?? new List<Book>();
    }
}