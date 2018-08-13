using Domain;

namespace Infrastructure
{
  public class Market : IMarket
  {
    public string Name { get; set; }
    public string Address { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Day { get; set; }
  }
}