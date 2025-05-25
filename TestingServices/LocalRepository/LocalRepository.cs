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
            var cmd = new SqliteCommand("INSERT INTO Customers (CustomerID, CompanyName, ContactName) VALUES (@CustomerID, @CompanyName, @ContactName)", connection);
            cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
            cmd.Parameters.AddWithValue("@CompanyName", customer.CompanyName);
            cmd.Parameters.AddWithValue("@ContactName", customer.ContactName ?? (object)DBNull.Value);

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
            var cmd = new SqliteCommand("INSERT INTO Orders (CustomerID, EmployeeID, OrderDate) VALUES (@CustomerID, @EmployeeID, @OrderDate); SELECT last_insert_rowid();", connection);
            cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
            cmd.Parameters.AddWithValue("@EmployeeID", order.EmployeeID);
            cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);

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
}
