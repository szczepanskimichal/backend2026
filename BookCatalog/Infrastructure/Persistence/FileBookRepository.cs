using System.Text.Json;
using Application.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Persistence;

public class FileBookRepository : IBookRepository
{
    private readonly string _filePath;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    public FileBookRepository(IHostEnvironment environment)
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

    public async Task AddAsync(Book book)
    {
        var books = await LoadBooksAsync();
        var nextId = books.Count == 0 ? 1 : books.Max(existingBook => existingBook.Id) + 1;

        if (book.Id <= 0 || books.Any(existingBook => existingBook.Id == book.Id))
        {
            book.Id = nextId;
        }

        books.Add(book);
        await SaveBooksAsync(books);
    }

    public async Task<bool> UpdateAsync(Book book)
    {
        var books = await LoadBooksAsync();
        var existingBook = books.FirstOrDefault(existing => existing.Id == book.Id);

        if (existingBook is null)
        {
            return false;
        }

        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.Year = book.Year;
        existingBook.IsAvailable = book.IsAvailable;

        await SaveBooksAsync(books);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var books = await LoadBooksAsync();
        var removedCount = books.RemoveAll(book => book.Id == id);

        if (removedCount == 0)
        {
            return false;
        }

        await SaveBooksAsync(books);
        return true;
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

    private async Task SaveBooksAsync(List<Book> books)
    {
        var directoryPath = Path.GetDirectoryName(_filePath);

        if (!string.IsNullOrWhiteSpace(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var json = JsonSerializer.Serialize(books, JsonSerializerOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }
}