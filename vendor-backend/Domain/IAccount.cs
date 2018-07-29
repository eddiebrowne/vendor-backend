namespace Domain
{
  public interface IAccount
  {
    string Name { get; set; }
    string Password { get; set; }
    string Email { get; set; }
  }
}