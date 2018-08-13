namespace Domain
{
  public interface IMarket
  {
    string Name { get; set; }
    string Address { get; set; }
    string StartTime { get; set; }
    string EndTime { get; set; }
  }
}