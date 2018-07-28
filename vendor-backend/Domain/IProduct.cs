namespace Domain
{
  public interface IProduct
  {
    string Name { get; set; }
    int Quantity { get; set; }
    decimal Price { get; set; }
    UnitType UnitType { get; set; }
  }
}