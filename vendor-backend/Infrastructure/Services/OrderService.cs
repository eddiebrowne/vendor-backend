using Domain;
using Domain.Services;

namespace Infrastructure.Services
{
  public class OrderService : IOrderService
  {
    private readonly IOrderRepository _repository;
    
    public OrderService(IOrderRepository repository)
    {
      _repository = repository;
    }
    
    public bool ProcessOrder(IOrder order)
    {
      throw new System.NotImplementedException();
    }
  }
}