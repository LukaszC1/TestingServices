using Google.Protobuf.WellKnownTypes;
using GrpcServiceProto;

namespace GrpcService.Client
{
    public class GrpcServiceClient(GrpcServiceProto.GrpcService.GrpcServiceClient client)
    {
        public async Task<ProductListResponse> GetAllProductsAsync()
        {
            return await client.GetAllProductsAsync(new Empty());
        }

        public async Task<CustomerListResponse> GetAllCustomersAsync()
        {
            return await client.GetAllCustomersAsync(new Empty());
        }

        public async Task<CustomerResponse> GetCustomerByIdAsync(string customerId)
        {
            var request = new CustomerRequest { CustomerId = customerId };
            return await client.GetCustomerByIdAsync(request);
        }

        public async Task<OrderListResponse> GetAllOrdersAsync()
        {
            return await client.GetAllOrdersAsync(new Empty());
        }

        public async Task<OrderDetailListResponse> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            var request = new OrderRequest { OrderId = orderId };
            return await client.GetOrderDetailsByOrderIdAsync(request);
        }

        public async Task<AddCustomerReply> AddCustomerAsync(Customer customer)
        {
            return await client.AddCustomerAsync(customer);
        }

        public async Task<EmployeeListResponse> GetAllEmployeesAsync()
        {
            return await client.GetAllEmployeesAsync(new Empty());
        }

        public async Task<AddOrderReply> AddOrderAsync(Order order)
        {
            return await client.AddOrderAsync(order);
        }
    }
}
}