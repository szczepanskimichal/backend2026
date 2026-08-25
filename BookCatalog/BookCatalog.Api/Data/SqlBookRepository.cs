using BookCatalog.Api.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BookCatalog.Api.Data;

public class SqlBookRepository : IBookRepository
{
    private readonly string _connectionString;

    public SqlBookRepository(IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString("BookCatalog")
            ?? throw new InvalidOperationException(
                "Brak connection stringa BookCatalog.");
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        const string sql = """
                           SELECT Id, 
                           Title, 
                           Author, 
                           [Year], 
                           IsAvailable
                           FROM Books;
                           """;

        await using var connection =
            new SqlConnection(_connectionString);

        return await connection.QueryAsync<Book>(sql);
    }

    public async Task<Book?> FindAsync(int id)
    {
        const string sql = """
                           SELECT Id, 
                           Title, 
                           Author, 
                           [Year], 
                           IsAvailable
                           FROM Books
                           WHERE Id = @Id;
                           """;

        await using var connection =
            new SqlConnection(_connectionString);

        return await connection.QuerySingleOrDefaultAsync<Book>(
            sql,
            new { Id = id });
    }

    public async Task<IEnumerable<Book>> FindByAuthorAsync(string author)
    {
        const string sql = """
                           SELECT Id, 
                           Title, 
                           Author, 
                           [Year], 
                           IsAvailable
                           FROM Books
                           WHERE Author = @Author;
                           """;

        await using var connection =
            new SqlConnection(_connectionString);

        return await connection.QueryAsync<Book>(
            sql,
            new { Author = author });
    }
}

