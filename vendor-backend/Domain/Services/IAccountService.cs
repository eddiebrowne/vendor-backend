namespace Domain.Services
{
  public interface IAccountService
  {
    long Create(IAccount account);
    IAccount GetVendorAccount(string email, string password);
    IAccount GetVendorFromToken(string token);
    string GenerateToken(IAccount account);
  }
}