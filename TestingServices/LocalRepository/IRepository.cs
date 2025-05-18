using LocalRepository.DTO;

namespace LocalRepository
{
    public interface IRepository
    {
        Task<Product?>? GetProductByIdAsync(int productId);
        Task<IEnumerable<Product?>> GetAllProductsAsync();
        Task<Customer?> GetCustomerByIdAsync(string customerId);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
    }
}
