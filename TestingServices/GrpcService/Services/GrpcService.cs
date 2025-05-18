using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using LocalRepository;
using GrpcServiceProto;

namespace GrpcService.Services
{
    public class GrpcService(IRepository repository) : GrpcServiceProto.GrpcService.GrpcServiceBase
    {
        public override async Task<ProductResponse> GetProductById(ProductRequest request, ServerCallContext context)
        {
            var product = await repository.GetProductByIdAsync(request.ProductId);

            if (product == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Product {request.ProductId} not found"));

            return new ProductResponse
            {
                ProductId = product.ProductID,
                ProductName = product.ProductName ?? "",
                QuantityPerUnit = product.QuantityPerUnit ?? "",
                UnitPrice = (double)(product.UnitPrice ?? 0)
            };
        }

        public override async Task<ProductListResponse> GetAllProducts(Empty request, ServerCallContext context)
        {
            var products = await repository.GetAllProductsAsync();
            var response = new ProductListResponse();
            foreach (var product in products.Where(p => p != null))
            {
                response.Products.Add(new ProductResponse
                {
                    ProductId = product!.ProductID,
                    ProductName = product.ProductName ?? "",
                    QuantityPerUnit = product.QuantityPerUnit ?? "",
                    UnitPrice = (double)(product.UnitPrice ?? 0)
                });
            }
            return response;
        }

        public override async Task<CustomerResponse> GetCustomerById(CustomerRequest request, ServerCallContext context)
        {
            var customer = await repository.GetCustomerByIdAsync(request.CustomerId);

            if (customer == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Customer {request.CustomerId} not found"));

            return new CustomerResponse
            {
                CustomerId = customer.CustomerID ?? "",
                CompanyName = customer.CompanyName ?? "",
                ContactName = customer.ContactName ?? ""
            };
        }

        public override async Task<CustomerListResponse> GetAllCustomers(Empty request, ServerCallContext context)
        {
            var customers = await repository.GetAllCustomersAsync();
            var response = new CustomerListResponse();
            foreach (var customer in customers)
            {
                response.Customers.Add(new CustomerResponse
                {
                    CustomerId = customer.CustomerID ?? "",
                    CompanyName = customer.CompanyName ?? "",
                    ContactName = customer.ContactName ?? ""
                });
            }
            return response;
        }

        public override async Task<OrderResponse> GetOrderById(OrderRequest request, ServerCallContext context)
        {
            var order = await repository.GetOrderByIdAsync(request.OrderId);

            if (order == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Order {request.OrderId} not found"));

            return new OrderResponse
            {
                OrderId = order.OrderID,
                CustomerId = order.CustomerID ?? "",
                OrderDate = order.OrderDate?.ToString("yyyy-MM-dd") ?? ""
            };
        }

        public override async Task<OrderListResponse> GetAllOrders(Empty request, ServerCallContext context)
        {
            var orders = await repository.GetAllOrdersAsync();
            var response = new OrderListResponse();
            foreach (var order in orders)
            {
                response.Orders.Add(new OrderResponse
                {
                    OrderId = order.OrderID,
                    CustomerId = order.CustomerID ?? "",
                    OrderDate = order.OrderDate?.ToString("yyyy-MM-dd") ?? ""
                });
            }
            return response;
        }

        public override async Task<OrderDetailListResponse> GetOrderDetailsByOrderId(OrderRequest request, ServerCallContext context)
        {
            var orderDetails = await repository.GetOrderDetailsByOrderIdAsync(request.OrderId);
            var response = new OrderDetailListResponse();
            foreach (var detail in orderDetails)
            {
                response.Details.Add(new OrderDetailResponse
                {
                    OrderId = detail.OrderID,
                    ProductId = detail.ProductID,
                    UnitPrice = (double)detail.UnitPrice,
                    Quantity = detail.Quantity,
                    Discount = detail.Discount
                });
            }
            return response;
        }
    }
}