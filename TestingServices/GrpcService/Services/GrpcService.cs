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
                    SupplierId = product.SupplierID ?? 0,
                    CategoryId = product.CategoryID ?? 0,
                    QuantityPerUnit = product.QuantityPerUnit ?? "",
                    UnitPrice = (double)(product.UnitPrice ?? 0),
                    UnitsInStock = product.UnitsInStock ?? 0,
                    UnitsOnOrder = product.UnitsOnOrder ?? 0,
                    ReorderLevel = product.ReorderLevel ?? 0,
                    Discontinued = product.Discontinued
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
                    ContactName = customer.ContactName ?? "",
                    ContactTitle = customer.ContactTitle ?? "",
                    Address = customer.Address ?? "",
                    City = customer.City ?? "",
                    Region = customer.Region ?? "",
                    PostalCode = customer.PostalCode ?? "",
                    Country = customer.Country ?? "",
                    Phone = customer.Phone ?? "",
                    Fax = customer.Fax ?? ""
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
                ContactName = customer.ContactName ?? "",
                ContactTitle = customer.ContactTitle ?? "",
                Address = customer.Address ?? "",
                City = customer.City ?? "",
                Region = customer.Region ?? "",
                PostalCode = customer.PostalCode ?? "",
                Country = customer.Country ?? "",
                Phone = customer.Phone ?? "",
                Fax = customer.Fax ?? ""
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
                    EmployeeId = order.EmployeeID ?? 0,
                    OrderDate = order.OrderDate?.ToString("yyyy-MM-dd") ?? "",
                    RequiredDate = order.RequiredDate?.ToString("yyyy-MM-dd") ?? "",
                    ShippedDate = order.ShippedDate?.ToString("yyyy-MM-dd") ?? "",
                    ShipVia = order.ShipVia ?? 0,
                    Freight = (double)(order.Freight ?? 0),
                    ShipName = order.ShipName ?? "",
                    ShipAddress = order.ShipAddress ?? "",
                    ShipCity = order.ShipCity ?? "",
                    ShipRegion = order.ShipRegion ?? "",
                    ShipPostalCode = order.ShipPostalCode ?? "",
                    ShipCountry = order.ShipCountry ?? ""
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
                    FirstName = employee.FirstName ?? "",
                    LastName = employee.LastName ?? ""
                });
            }

            return response;
        }

        public override async Task<AddCustomerReply> AddCustomer(Customer request, ServerCallContext context)
        {
            var customer = new LocalRepository.DTO.Customer
            {
                CustomerID = request.CustomerID,
                CompanyName = request.CompanyName,
                ContactName = request.ContactName,
                ContactTitle = request.ContactTitle,
                Address = request.Address,
                City = request.City,
                Region = request.Region,
                PostalCode = request.PostalCode,
                Country = request.Country,
                Phone = request.Phone,
                Fax = request.Fax
            };

            var result = await repository.AddCustomerAsync(customer);
            return new AddCustomerReply { Success = result };
        }

        public override async Task<AddOrderReply> AddOrder(Order request, ServerCallContext context)
        {
            var order = new LocalRepository.DTO.Order
            {
                OrderID = request.OrderId,
                CustomerID = request.CustomerID,
                EmployeeID = request.EmployeeID == 0 ? null : request.EmployeeID,
                OrderDate = string.IsNullOrEmpty(request.OrderDate) ? null : DateTime.Parse(request.OrderDate),
                RequiredDate = string.IsNullOrEmpty(request.RequiredDate) ? null : DateTime.Parse(request.RequiredDate),
                ShippedDate = string.IsNullOrEmpty(request.ShippedDate) ? null : DateTime.Parse(request.ShippedDate),
                ShipVia = request.ShipVia == 0 ? null : request.ShipVia,
                Freight = (decimal?)request.Freight,
                ShipName = request.ShipName,
                ShipAddress = request.ShipAddress,
                ShipCity = request.ShipCity,
                ShipRegion = request.ShipRegion,
                ShipPostalCode = request.ShipPostalCode,
                ShipCountry = request.ShipCountry
            };

            var result = await repository.AddOrderAsync(order);
            return new AddOrderReply { OrderId = result };
        }

        public override async Task<OrdersWithDetailsResponse> GetOrdersWithDetails(OrdersWithDetailsRequest request, ServerCallContext context)
        {
            var orders = await repository.GetOrdersWithDetailsAsync(request.HasOrderId ? request.OrderId : null);
            var response = new OrdersWithDetailsResponse();
            foreach (var order in orders)
            {
                var orderMsg = new OrderWithDetails
                {
                    OrderId = order.OrderID,
                    CustomerId = order.CustomerID ?? "",
                    EmployeeId = order.EmployeeID,
                    OrderDate = order.OrderDate.ToString("yyyy-MM-dd")
                };

                if (order.OrderDetails != null)
                {
                    foreach (var detail in order.OrderDetails)
                    {
                        orderMsg.OrderDetails.Add(new OrderDetailResponse
                        {
                            OrderId = detail.OrderID,
                            ProductId = detail.ProductID,
                            UnitPrice = (double)detail.UnitPrice,
                            Quantity = detail.Quantity,
                            Discount = detail.Discount
                        });
                    }
                }
                response.Orders.Add(orderMsg);
            }
            return response;
        }

        public override async Task<CustomersWithOrdersResponse> GetCustomersWithOrders(CustomersWithOrdersRequest request, ServerCallContext context)
        {
            var customers = await repository.GetCustomerWithOrdersAsync(string.IsNullOrEmpty(request.CustomerId) ? null : request.CustomerId);
            var response = new CustomersWithOrdersResponse();
            foreach (var customer in customers)
            {
                var customerMsg = new CustomerWithOrders
                {
                    CustomerId = customer.CustomerID ?? "",
                    CompanyName = customer.CompanyName ?? "",
                    ContactName = customer.ContactName ?? "",
                };

                if (customer.Orders != null)
                {
                    foreach (var order in customer.Orders)
                    {
                        var orderMsg = new OrderWithDetails
                        {
                            OrderId = order.OrderID,
                            CustomerId = order.CustomerID ?? "",
                            EmployeeId = order.EmployeeID,
                            OrderDate = order.OrderDate.ToString("yyyy-MM-dd")
                        };

                        if (order.OrderDetails != null)
                        {
                            foreach (var detail in order.OrderDetails)
                            {
                                orderMsg.OrderDetails.Add(new OrderDetailResponse
                                {
                                    OrderId = detail.OrderID,
                                    ProductId = detail.ProductID,
                                    UnitPrice = (double)detail.UnitPrice,
                                    Quantity = detail.Quantity,
                                    Discount = detail.Discount
                                });
                            }
                        }
                        customerMsg.Orders.Add(orderMsg);
                    }
                }
                response.Customers.Add(customerMsg);
            }
            return response;
        }
    }
}