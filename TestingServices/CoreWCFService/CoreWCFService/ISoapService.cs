using LocalRepository.DTO;

namespace CoreWCFService
{
    [ServiceContract]
    public interface ISoapService
    {
        [OperationContract]
        Task<Product> GetProductByIdAsync(int productId);

        [OperationContract]
        Task<IEnumerable<Product?>> GetAllProductsAsync();

        [OperationContract]
        Task<Customer> GetCustomerByIdAsync(string customerId);

        [OperationContract]
        Task<IEnumerable<Customer>> GetAllCustomersAsync();

        [OperationContract]
        Task<Order> GetOrderByIdAsync(int orderId);

        [OperationContract]
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        [OperationContract]
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
    }
}
