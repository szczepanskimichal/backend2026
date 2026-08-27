namespace ProductInventory.Core.DomainServices;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> FindAsync(int id);
    Task<int> CreateAsync(Product product);
    Task<Product> UpdateStockAsync(int id, int newStockCount);
    Task<Product> DeleteAsync(int id);
}