using Microsoft.Extensions.Primitives;

namespace Domain
{
  public class AccountService : IAccountService
  {
    private readonly IDatabase _database;
    
    public AccountService(IDatabase database)
    {
      _database = database;
    }
    
    public int Create(IAccount account)
    {
      
      
      return 0;
      
    }

    public static IAccount GetVendorFromToken(string token)
    {
      return null;
    }
  }
}