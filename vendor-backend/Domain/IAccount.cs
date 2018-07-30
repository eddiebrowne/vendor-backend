namespace Domain
{
  public interface IAccount : ILogin
  {
    string Name { get; set; }
  }
}