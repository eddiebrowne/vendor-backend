using Domain;

namespace Infrastructure
{
  public class Account : IAccount
  {
    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
  }
}