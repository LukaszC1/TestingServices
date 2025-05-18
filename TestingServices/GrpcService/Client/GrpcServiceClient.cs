using Google.Protobuf.WellKnownTypes;
using GrpcServiceProto;

namespace GrpcService.Client
{
    public class GrpcServiceClient(GrpcServiceProto.GrpcService.GrpcServiceClient client) : GrpcServiceProto.GrpcService.GrpcServiceClient
    {
        public async Task<ProductResponse> GetProductByIdAsync(int productId)
        {
            var request = new ProductRequest { ProductId = productId };
            return await client.GetProductByIdAsync(request);
        }

        public async Task<ProductListResponse> GetAllProductsAsync()
        {
            return await client.GetAllProductsAsync(new Empty());
        }

        public async Task<CustomerResponse> GetCustomerByIdAsync(string customerId)
        {
            var request = new CustomerRequest { CustomerId = customerId };
            return await client.GetCustomerByIdAsync(request);
        }

        public async Task<CustomerListResponse> GetAllCustomersAsync()
        {
            return await client.GetAllCustomersAsync(new Empty());
        }

        public async Task<OrderResponse> GetOrderByIdAsync(int orderId)
        {
            var request = new OrderRequest { OrderId = orderId };
            return await client.GetOrderByIdAsync(request);
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
    }
}