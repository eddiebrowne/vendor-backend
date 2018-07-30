using Domain;

namespace Infrastructure
{
  public class Login : ILogin
  {
    public string Email { get; set; }
    public string Password { get; set; }
  }
}