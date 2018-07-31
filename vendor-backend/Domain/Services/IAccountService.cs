namespace Domain
{
  public interface IAccountService
  {
    int Create(IAccount account);
    IAccount GetVendorAccount(string email, string password);
    IAccount GetVendorFromToken(string token);
    string CreateToken(IAccount account);
  }
}