using Microsoft.Extensions.Primitives;

namespace Domain
{
  public class AccountService : IAccountService
  {
    private readonly IAccountRepository _repository;
    
    public AccountService(IAccountRepository repository)
    {
      _repository = repository;
    }
    
    public int Create(IAccount account)
    {
      
      
      return 0;
      
    }

    public IAccount GetVendorAccount(string email, string password)
    {
      return _repository.GetAccount(email, password);
    }

    public static IAccount GetVendorFromToken(string token)
    {
      return null;
    }
  }
}