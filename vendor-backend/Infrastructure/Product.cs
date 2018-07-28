using Domain;

namespace Infrastructure
{
  public class Product : IProduct
  {
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public UnitType UnitType { get; set; }

  }
}