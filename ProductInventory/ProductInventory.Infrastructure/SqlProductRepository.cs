using Dapper;
using Microsoft.Data.SqlClient;
using ProductInventory.Core;
using ProductInventory.Core.DomainServices;

namespace ProductInventory.Infrastructure
{
    public class SqlProductRepository(string connectionString) : IProductRepository
    {
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            const string sql =
                """
                SELECT Id, Name, ProductCode, StockCount
                FROM Products
                ORDER BY Id;
                """;

            await using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Product>(sql);
        }

        public async Task<Product?> FindAsync(int id)
        {
            const string sql =
                """
                SELECT Id, Name, ProductCode, StockCount
                FROM Products
                WHERE Id = @Id;
                """;

            await using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
        }

        public async Task<int> CreateAsync(Product product)
        {
            const string sql =
                """
                INSERT INTO Products
                    (Name, ProductCode, StockCount)
                OUTPUT INSERTED.Id
                VALUES
                    (@Name, @ProductCode, @StockCount);
                """;

            await using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleAsync<int>(sql, product);
        }

        public async Task<Product?> UpdateStockAsync(int id, int newStockCount)
        {
            const string sql =
                """
                UPDATE Products
                SET StockCount = @StockCount
                OUTPUT
                    INSERTED.Id,
                    INSERTED.Name,
                    INSERTED.ProductCode,
                    INSERTED.StockCount
                WHERE Id = @Id;
                """;

            await using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<Product>(
                sql,
                new
                {
                    Id = id,
                    StockCount = newStockCount
                });
        }

        public async Task<Product?> DeleteAsync(int id)
        {
            const string sql =
                """
                DELETE FROM Products
                OUTPUT
                    DELETED.Id,
                    DELETED.Name,
                    DELETED.ProductCode,
                    DELETED.StockCount
                WHERE Id = @Id;
                """;

            await using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
        }
    }
}