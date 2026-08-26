
using BookCatalog.Api.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


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

    public async Task AddAsync(Book book)
    {
        const string sql = """
                           INSERT INTO Books
                           (
                               Id,
                               Title,
                               Author,
                               [Year],
                               IsAvailable
                           )
                           VALUES
                           (
                               @Id,
                               @Title,
                               @Author,
                               @Year,
                               @IsAvailable
                           );
                           """;

        await using var connection =
            new SqlConnection(_connectionString);

        await connection.ExecuteAsync(sql, book);
    }

    public async Task<bool> UpdateAsync(Book book)
    {
        const string sql = """
                           UPDATE Books
                           SET
                               Title = @Title,
                               Author = @Author,
                               [Year] = @Year,
                               IsAvailable = @IsAvailable
                           WHERE Id = @Id;
                           """;

        await using var connection =
            new SqlConnection(_connectionString);

        var affectedRows =
            await connection.ExecuteAsync(sql, book);

        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = """
                           DELETE FROM Books
                           WHERE Id = @Id;
                           """;

        await using var connection =
            new SqlConnection(_connectionString);

        var affectedRows =
            await connection.ExecuteAsync(
                sql,
                new { Id = id });

        return affectedRows > 0;
    }
}

