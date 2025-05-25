using LocalRepository.DTO;

namespace CoreWCFService
{
    [ServiceContract]
    public interface ISoapService
    {
        [OperationContract]
        Task<IEnumerable<Product?>> GetAllProductsAsync();

        [OperationContract]
        Task<Customer> GetCustomerByIdAsync(string customerId);

        [OperationContract]
        Task<IEnumerable<Customer>> GetAllCustomersAsync();

        [OperationContract]
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        [OperationContract]
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);

        [OperationContract]
        Task<bool> AddCustomerAsync(Customer customer);

        [OperationContract]
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();

        [OperationContract]
        Task<int> AddOrderAsync(Order order);
    }
}
