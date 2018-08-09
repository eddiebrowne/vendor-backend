namespace Domain
{
  public interface IProduct
  {
    string Name { get; set; }
    int Quantity { get; set; }
    decimal Price { get; set; }
    string UnitType { get; set; }
    int Count { get; set; }
    int Order { get; set; }
    string Picture { get; set; }
    int Id { get; set; }
  }
}