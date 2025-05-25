using LocalRepository.DTO;

namespace LocalRepository
{
    public interface IRepository
    {
        Task<Customer?> GetCustomerByIdAsync(string customerId);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<bool> AddCustomerAsync(Customer customer);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<int> AddOrderAsync(Order order);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task<IEnumerable<OrderWithDetails>> GetOrdersWithDetailsAsync(int? orderId = null);
        Task<IEnumerable<CustomerWithOrders>> GetCustomerWithOrdersAsync(string? customerId = null);
    }
}
