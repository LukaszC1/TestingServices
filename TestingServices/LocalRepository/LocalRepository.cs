using LocalRepository.DTO;
using Microsoft.Data.Sqlite;

namespace LocalRepository
{
    public class LocalRepository : IRepository
    {
        private readonly string _connectionString;

        public LocalRepository(string connectionString)
        {
            _connectionString = connectionString;
            EnableWriteAheadLogging();
        }

        public LocalRepository()
        {
            _connectionString = "Data Source=../northwind.db;Pooling=True;";
            EnableWriteAheadLogging();
        }
        
        private void EnableWriteAheadLogging()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var command = new SqliteCommand("PRAGMA journal_mode=WAL;", connection);
            command.ExecuteNonQuery();
        }

        public async Task<Customer?> GetCustomerByIdAsync(string customerId)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            var cmd = new SqliteCommand("SELECT * FROM Customers WHERE CustomerID = @CustomerID", connection);
            cmd.Parameters.AddWithValue("@CustomerID", customerId);

            var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapCustomer(reader) : null;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            var customers = new List<Customer>();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            var cmd = new SqliteCommand("SELECT * FROM Customers", connection);
            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                customers.Add(MapCustomer(reader));

            return customers;
        }

        public async Task<bool> AddCustomerAsync(Customer customer)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            var cmd = new SqliteCommand(@"
                INSERT INTO Customers (
                    CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax
                ) VALUES (
                    @CustomerID, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax
                )", connection);

            cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
            cmd.Parameters.AddWithValue("@CompanyName", customer.CompanyName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ContactName", customer.ContactName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ContactTitle", customer.ContactTitle ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", customer.Address ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@City", customer.City ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Region", customer.Region ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@PostalCode", customer.PostalCode ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Country", customer.Country ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Phone", customer.Phone ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Fax", customer.Fax ?? (object)DBNull.Value);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            var employees = new List<Employee>();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            var cmd = new SqliteCommand("SELECT * FROM Employees", connection);
            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                employees.Add(MapEmployee(reader));

            return employees;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var orders = new List<Order>();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            var cmd = new SqliteCommand("SELECT * FROM Orders", connection);
            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                orders.Add(MapOrder(reader));

            return orders;
        }

        public async Task<int> AddOrderAsync(Order order)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            var cmd = new SqliteCommand(@"
                INSERT INTO Orders (
                    CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry
                ) VALUES (
                    @CustomerID, @EmployeeID, @OrderDate, @RequiredDate, @ShippedDate, @ShipVia, @Freight, @ShipName, @ShipAddress, @ShipCity, @ShipRegion, @ShipPostalCode, @ShipCountry
                );
                SELECT last_insert_rowid();", connection);

            cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
            cmd.Parameters.AddWithValue("@EmployeeID", order.EmployeeID);
            cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@RequiredDate", order.RequiredDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ShippedDate", order.ShippedDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ShipVia", order.ShipVia ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Freight", order.Freight ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ShipName", order.ShipName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ShipAddress", order.ShipAddress ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ShipCity", order.ShipCity ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ShipRegion", order.ShipRegion ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ShipPostalCode", order.ShipPostalCode ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ShipCountry", order.ShipCountry ?? (object)DBNull.Value);

            var insertedId = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(insertedId);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            var cmd = new SqliteCommand("SELECT * FROM Products", connection);
            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                products.Add(MapProduct(reader));

            return products;
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            var details = new List<OrderDetail>();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            var cmd = new SqliteCommand("SELECT * FROM [Order Details] WHERE OrderID = @OrderID", connection);
            cmd.Parameters.AddWithValue("@OrderID", orderId);
            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                details.Add(MapOrderDetail(reader));

            return details;
        }

        public async Task<IEnumerable<OrderWithDetails>> GetOrdersWithDetailsAsync(int? orderId = null)
        {
            var ordersDict = new Dictionary<int, OrderWithDetails>();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                SELECT o.OrderID, o.CustomerID, o.EmployeeID, o.OrderDate,
                       od.ProductID, od.Quantity
                FROM Orders o
                LEFT JOIN [Order Details] od ON o.OrderID = od.OrderID
                " + (orderId.HasValue ? "WHERE o.OrderID = @OrderID" : "");

            var cmd = new SqliteCommand(sql, connection);
            if (orderId.HasValue)
                cmd.Parameters.AddWithValue("@OrderID", orderId.Value);

            var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var oid = reader.GetInt32(reader.GetOrdinal("OrderID"));
                if (!ordersDict.TryGetValue(oid, out var order))
                {
                    order = new OrderWithDetails
                    {
                        OrderID = oid,
                        CustomerID = reader["CustomerID"].ToString(),
                        EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                        OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                        OrderDetails = []
                    };
                    ordersDict[oid] = order;
                }

                if (!reader.IsDBNull(reader.GetOrdinal("ProductID")))
                {
                    order.OrderDetails.Add(new OrderDetail
                    {
                        OrderID = oid,
                        ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                        Quantity = reader.GetInt16(reader.GetOrdinal("Quantity"))
                    });
                }
            }
            await reader.CloseAsync();

            return ordersDict.Values;
        }

        public async Task<IEnumerable<CustomerWithOrders>> GetCustomerWithOrdersAsync(string? customerId = null)
        {
            var customersDict = new Dictionary<string, CustomerWithOrders>();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                SELECT c.CustomerID, c.CompanyName, c.ContactName,
                       o.OrderID, o.EmployeeID, o.OrderDate,
                       od.ProductID, od.Quantity
                FROM Customers c
                LEFT JOIN Orders o ON c.CustomerID = o.CustomerID
                LEFT JOIN [Order Details] od ON o.OrderID = od.OrderID
                " + (customerId != null ? "WHERE c.CustomerID = @CustomerID" : "");

            var cmd = new SqliteCommand(sql, connection);
            if (customerId != null)
                cmd.Parameters.AddWithValue("@CustomerID", customerId);

            var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var cid = reader["CustomerID"].ToString();
                if (!customersDict.TryGetValue(cid, out var customer))
                {
                    customer = new CustomerWithOrders
                    {
                        CustomerID = cid,
                        CompanyName = reader["CompanyName"].ToString(),
                        ContactName = reader["ContactName"].ToString(),
                        Orders = []
                    };
                    customersDict[cid] = customer;
                }

                // If there is an order row (OrderID not null)
                if (!reader.IsDBNull(reader.GetOrdinal("OrderID")))
                {
                    var oid = reader.GetInt32(reader.GetOrdinal("OrderID"));
                    var order = customer.Orders.FirstOrDefault(o => o.OrderID == oid);
                    if (order == null)
                    {
                        order = new OrderWithDetails
                        {
                            OrderID = oid,
                            CustomerID = cid,
                            EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                            OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                            OrderDetails = new List<OrderDetail>()
                        };
                        customer.Orders.Add(order);
                    }

                    // If there is a detail row (ProductID not null)
                    if (!reader.IsDBNull(reader.GetOrdinal("ProductID")))
                    {
                        order.OrderDetails.Add(new OrderDetail
                        {
                            OrderID = oid,
                            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                            Quantity = reader.GetInt16(reader.GetOrdinal("Quantity"))
                        });
                    }
                }
            }
            await reader.CloseAsync();

            return customersDict.Values;
        }

        private static Customer MapCustomer(SqliteDataReader reader) => new Customer
        {
            CustomerID = reader["CustomerID"].ToString(),
            CompanyName = reader["CompanyName"].ToString(),
            ContactName = reader["ContactName"].ToString()
        };

        private static Employee MapEmployee(SqliteDataReader reader) => new Employee
        {
            EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
            FirstName = reader["FirstName"].ToString(),
            LastName = reader["LastName"].ToString()
        };

        private static Order MapOrder(SqliteDataReader reader) => new Order
        {
            OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
            CustomerID = reader["CustomerID"].ToString(),
            EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
            OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate"))
        };

        private static Product MapProduct(SqliteDataReader reader) => new Product
        {
            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
            ProductName = reader["ProductName"].ToString()
        };

        private static OrderDetail MapOrderDetail(SqliteDataReader reader) => new OrderDetail
        {
            OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
            Quantity = reader.GetInt16(reader.GetOrdinal("Quantity"))
        };
    }
}
