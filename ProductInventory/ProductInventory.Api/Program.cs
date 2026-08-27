using ProductInventory.Core;
using ProductInventory.Core.ApplicationService;
using ProductInventory.Core.DomainServices;
using ProductInventory.DTO;
using ProductInventory.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ProductInventory")
    ?? throw new InvalidOperationException(
        "Connection string 'ProductInventory' mangler i konfigurasjonen.");

builder.Services.AddScoped<IProductRepository>(_ => new SqlProductRepository(connectionString));
builder.Services.AddScoped<ProductService>();

var app = builder.Build();

app.MapGet(
    "/products",
    async (ProductService service) =>
    {
        var products = await service.GetAllAsync();
        return Results.Ok(products);
    });

app.MapGet(
    "/products/{id:int}",
    async (int id, ProductService service) =>
    {
        var product = await service.FindAsync(id);

        return product is null
            ? Results.NotFound("Produktet finnes ikke.")
            : Results.Ok(product);
    });

app.MapPost("/products", async (CreateProductDto dto, ProductService service) =>
    {
        var product = new Product
        {
            Name = dto.Name,
            ProductCode = dto.ProductCode,
            StockCount = dto.StockCount
        };

        var result = await service.CreateProductAsync(product);

        if (!result.IsSuccess) return Results.BadRequest(result.ErrorMessage);

        return Results.Created($"/products/{result.Value!.Id}", result.Value);
    });

app.MapPatch(
    "/products/{id:int}/stock",
    async (int id, UpdateStockDto dto, ProductService service) =>
    {
        var result = await service.UpdateStockAsync(id, dto.StockCount);

        if (!result.IsSuccess)
        {
            return Results.BadRequest(result.ErrorMessage);
        }

        return Results.Ok(result.Value);
    });

app.MapDelete(
    "/products/{id:int}",
    async (int id, ProductService service) =>
    {
        var result = await service.DeleteAsync(id);

        if (!result.IsSuccess)
        {
            return Results.NotFound(result.ErrorMessage);
        }

        return Results.Ok(result.Value);
    });

app.Run();