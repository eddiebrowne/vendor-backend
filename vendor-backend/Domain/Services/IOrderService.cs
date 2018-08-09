namespace Domain.Services
{
  public interface IOrderService
  {
    bool ProcessOrder(IOrder order);
  }
}