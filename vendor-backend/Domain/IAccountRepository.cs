namespace Domain
{
  public interface IAccountRepository
  {
    IAccount GetAccount(string email, string password);
  }
}