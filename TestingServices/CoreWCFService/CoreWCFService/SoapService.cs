using LocalRepository.DTO;
using LocalRepository;

namespace CoreWCFService;

public class SoapService(IRepository repository) : ISoapService
{
    public Task<Product>? GetProductByIdAsync(int productId) => repository.GetProductByIdAsync(productId);

    public Task<IEnumerable<Product?>> GetAllProductsAsync() => repository.GetAllProductsAsync();

    public Task<Customer> GetCustomerByIdAsync(string customerId) => repository.GetCustomerByIdAsync(customerId);

    public Task<IEnumerable<Customer>> GetAllCustomersAsync() => repository.GetAllCustomersAsync();

    public Task<Order> GetOrderByIdAsync(int orderId) => repository.GetOrderByIdAsync(orderId);

    public Task<IEnumerable<Order>> GetAllOrdersAsync() => repository.GetAllOrdersAsync();

    public Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId) => repository.GetOrderDetailsByOrderIdAsync(orderId);
}