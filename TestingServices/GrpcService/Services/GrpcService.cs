using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcServiceProto;
using LocalRepository;

using Customer = GrpcServiceProto.Customer;
using Employee = GrpcServiceProto.Employee;
using Order = GrpcServiceProto.Order;

namespace GrpcService.Services
{
    public class GrpcService(IRepository repository) : GrpcServiceProto.GrpcService.GrpcServiceBase
    {
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

        public override async Task<EmployeeListResponse> GetAllEmployees(Empty request, ServerCallContext context)
        {
            var employees = await repository.GetAllEmployeesAsync();
            var response = new EmployeeListResponse();
            foreach (var employee in employees)
            {
                response.Employees.Add(new Employee
                {
                    EmployeeId = employee.EmployeeID,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName
                });
            }

            return response;
        }

        public override async Task<AddCustomerReply> AddCustomer(Customer request, ServerCallContext context)
        {
            LocalRepository.DTO.Customer customer = new LocalRepository.DTO.Customer()
            {
                CustomerID = request.CustomerID,
                CompanyName = request.CompanyName,
                ContactName = request.ContactName
            };

            var result = await repository.AddCustomerAsync(customer);
            return new AddCustomerReply { Success = result };
        }

        public override async Task<AddOrderReply> AddOrder(Order request, ServerCallContext context)
        {
            var order = new LocalRepository.DTO.Order()
            {
                CustomerID = request.CustomerID,
                EmployeeID = request.EmployeeID,
                OrderDate = DateTime.Parse(request.OrderDate)
            };

            var orderId = await repository.AddOrderAsync(order);
            return new AddOrderReply { OrderId = orderId };
        }
    }
}