namespace BookCatalog.Api.Models;

public interface IBookRepository
{
   Task<IEnumerable<Book>> GetAllBooksAsync();
   Task<Book?> FindAsync(int id);
   Task<IEnumerable<Book>> FindByAuthorAsync(string author);
}