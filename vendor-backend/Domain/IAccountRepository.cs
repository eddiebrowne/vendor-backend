using Domain.Services;

namespace Domain
{
  public interface IAccountRepository
  {
    IAccount GetAccount(string email, string password);
    IAccount GetAccountFromToken(string token);
    int Create(IAccount account);
    void CreateDatabase(string accountName);
    void StoreToken(IAccount account, string token);
    IVendor GetVendor(int vendorId);
  }
}