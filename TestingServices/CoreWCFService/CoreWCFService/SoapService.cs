using LocalRepository.DTO;
using LocalRepository;

namespace CoreWCFService;

public class SoapService(IRepository repository) : ISoapService
{
    public Task<IEnumerable<Product?>> GetAllProductsAsync() => repository.GetAllProductsAsync();

    public Task<Customer> GetCustomerByIdAsync(string customerId) => repository.GetCustomerByIdAsync(customerId);

    public Task<IEnumerable<Customer>> GetAllCustomersAsync() => repository.GetAllCustomersAsync();

    public Task<IEnumerable<Order>> GetAllOrdersAsync() => repository.GetAllOrdersAsync();

    public Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId) => repository.GetOrderDetailsByOrderIdAsync(orderId);

    public Task<bool> AddCustomerAsync(Customer customer) => repository.AddCustomerAsync(customer);

    public Task<IEnumerable<Employee>> GetAllEmployeesAsync() => repository.GetAllEmployeesAsync();

    public Task<int> AddOrderAsync(Order order) => repository.AddOrderAsync(order);
}