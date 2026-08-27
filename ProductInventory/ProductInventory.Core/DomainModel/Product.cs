namespace ProductInventory.Core;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string ProductCode { get; set; } = "";

    public int StockCount { get; set; }
}