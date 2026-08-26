using Domain.Entities;

namespace Application.Abstractions;

public interface IBookRepository
{
   Task<IEnumerable<Book>> GetAllBooksAsync();
   Task<Book?> FindAsync(int id);
   Task<IEnumerable<Book>> FindByAuthorAsync(string author);
   Task AddAsync(Book book);
   Task<bool> UpdateAsync(Book book);

   Task<bool> DeleteAsync(int id);
}