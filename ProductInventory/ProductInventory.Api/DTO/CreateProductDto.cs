namespace ProductInventory.DTO;

public class CreateProductDto
{
    public string Name { get; set; } = "";

    public string ProductCode { get; set; } = "";

    public int StockCount { get; set; }
}