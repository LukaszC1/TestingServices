using LocalRepository.DTO;
using LocalRepository;

namespace CoreWCFService;

public class SoapService : ISoapService
{
    private readonly IRepository _repository;
    public SoapService(IRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Product?>> GetAllProductsAsync() => _repository.GetAllProductsAsync();

    public Task<Customer> GetCustomerByIdAsync(string customerId) => _repository.GetCustomerByIdAsync(customerId);

    public Task<IEnumerable<Customer>> GetAllCustomersAsync() => _repository.GetAllCustomersAsync();

    public Task<IEnumerable<Order>> GetAllOrdersAsync() => _repository.GetAllOrdersAsync();

    public Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId) => _repository.GetOrderDetailsByOrderIdAsync(orderId);

    public Task<bool> AddCustomerAsync(Customer customer) => _repository.AddCustomerAsync(customer);

    public Task<IEnumerable<Employee>> GetAllEmployeesAsync() => _repository.GetAllEmployeesAsync();

    public Task<int> AddOrderAsync(Order order) => _repository.AddOrderAsync(order);

    public Task<IEnumerable<OrderWithDetails>> GetOrdersWithDetailsAsync(int? orderId = null)
        => _repository.GetOrdersWithDetailsAsync(orderId);

    public Task<IEnumerable<CustomerWithOrders>> GetCustomerWithOrdersAsync(string? customerId = null)
        => _repository.GetCustomerWithOrdersAsync(customerId);
}