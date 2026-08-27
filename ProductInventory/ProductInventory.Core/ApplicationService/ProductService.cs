using ProductInventory.Core.DomainServices;

namespace ProductInventory.Core.ApplicationService
{
    public class ProductService(IProductRepository repo)
    {
        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return repo.GetAllAsync();
        }

        public Task<Product?> FindAsync(int id)
        {
            return repo.FindAsync(id);
        }

        public async Task<Result<Product>> CreateProductAsync(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                return Result<Product>.Failure("Navn må fylles ut.");
            }

            if (string.IsNullOrWhiteSpace(product.ProductCode))
            {
                return Result<Product>.Failure("Produktkode må fylles ut.");
            }

            if (product.StockCount < 0)
            {
                return Result<Product>.Failure("Lagerbeholdning kan ikke være negativ.");
            }

            var id = await repo.CreateAsync(product);
            product.Id = id;
            return Result<Product>.Success(product);
        }

        public async Task<Result<Product>> UpdateStockAsync(
            int id,
            int newStockCount)
        {
            if (newStockCount < 0)
            {
                return Result<Product>.Failure("Lagerbeholdning kan ikke være negativ.");
            }

            var product = await repo.UpdateStockAsync(id, newStockCount);

            if (product is null)
            {
                return Result<Product>.Failure("Produktet finnes ikke.");
            }

            return Result<Product>.Success(product);
        }

        public async Task<Result<Product>> DeleteAsync(int id)
        {
            var product = await repo.DeleteAsync(id);

            if (product is null)
            {
                return Result<Product>.Failure("Produktet finnes ikke.");
            }

            return Result<Product>.Success(product);
        }
    }
}