namespace Domain
{
  public interface IAccountRepository
  {
    IAccount GetAccount(string email, string password);
    IAccount GetAccountFromToken(string token);
    long Create(IAccount account);
    void CreateDatabase(string accountName);
  }
}