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

        public async Task<Product?>? GetProductByIdAsync(int productId)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            await using var cmd = new SqliteCommand("SELECT * FROM Products WHERE ProductID = @ProductID", connection);
            cmd.Parameters.AddWithValue("@ProductID", productId);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapProduct(reader);
            }

            return null;
        }

        public async Task<IEnumerable<Product?>> GetAllProductsAsync()
        {
            var products = new List<Product?>();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            await using var cmd = new SqliteCommand("SELECT * FROM Products", connection);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                products.Add(MapProduct(reader));
            }

            return products;
        }

        public async Task<Customer?> GetCustomerByIdAsync(string customerId)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            await using var cmd = new SqliteCommand("SELECT * FROM Customers WHERE CustomerID = @CustomerID", connection);
            cmd.Parameters.AddWithValue("@CustomerID", customerId);
            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapCustomer(reader);
            }

            return null;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            var customers = new List<Customer>();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            await using var cmd = new SqliteCommand("SELECT * FROM Customers", connection);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                customers.Add(MapCustomer(reader));
            }

            return customers;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            await using var cmd = new SqliteCommand("SELECT * FROM Orders WHERE OrderID = @OrderID", connection);
            cmd.Parameters.AddWithValue("@OrderID", orderId);
            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapOrder(reader);
            }

            return null;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var orders = new List<Order>();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            await using var cmd = new SqliteCommand("SELECT * FROM Orders", connection);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                orders.Add(MapOrder(reader));
            }

            return orders;
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            var details = new List<OrderDetail>();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            await using var cmd = new SqliteCommand("SELECT * FROM [Order Details] WHERE OrderID = @OrderID", connection);
            cmd.Parameters.AddWithValue("@OrderID", orderId);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                details.Add(MapOrderDetail(reader));
            }

            return details;
        }

        private static Product? MapProduct(SqliteDataReader reader)
        {
            return new Product
            {
                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                SupplierID = reader.IsDBNull(reader.GetOrdinal("SupplierID"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("SupplierID")),
                CategoryID = reader.IsDBNull(reader.GetOrdinal("CategoryID"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("CategoryID")),
                QuantityPerUnit = reader.IsDBNull(reader.GetOrdinal("QuantityPerUnit"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("QuantityPerUnit")),
                UnitPrice = reader.IsDBNull(reader.GetOrdinal("UnitPrice"))
                    ? null
                    : reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                UnitsInStock = reader.IsDBNull(reader.GetOrdinal("UnitsInStock"))
                    ? null
                    : reader.GetInt16(reader.GetOrdinal("UnitsInStock")),
                UnitsOnOrder = reader.IsDBNull(reader.GetOrdinal("UnitsOnOrder"))
                    ? null
                    : reader.GetInt16(reader.GetOrdinal("UnitsOnOrder")),
                ReorderLevel = reader.IsDBNull(reader.GetOrdinal("ReorderLevel"))
                    ? null
                    : reader.GetInt16(reader.GetOrdinal("ReorderLevel")),
                Discontinued = reader.GetBoolean(reader.GetOrdinal("Discontinued"))
            };
        }

        private static Customer MapCustomer(SqliteDataReader reader)
        {
            return new Customer
            {
                CustomerID = reader.GetString(reader.GetOrdinal("CustomerID")),
                CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                ContactName = reader.IsDBNull(reader.GetOrdinal("ContactName"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("ContactName")),
                ContactTitle = reader.IsDBNull(reader.GetOrdinal("ContactTitle"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("ContactTitle")),
                Address = reader.IsDBNull(reader.GetOrdinal("Address"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Address")),
                City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City")),
                Region = reader.IsDBNull(reader.GetOrdinal("Region"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Region")),
                PostalCode = reader.IsDBNull(reader.GetOrdinal("PostalCode"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("PostalCode")),
                Country = reader.IsDBNull(reader.GetOrdinal("Country"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Country")),
                Phone = reader.IsDBNull(reader.GetOrdinal("Phone"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Phone")),
                Fax = reader.IsDBNull(reader.GetOrdinal("Fax")) ? null : reader.GetString(reader.GetOrdinal("Fax"))
            };
        }

        private static Order MapOrder(SqliteDataReader reader)
        {
            return new Order
            {
                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                CustomerID = reader.GetString(reader.GetOrdinal("CustomerID")),
                EmployeeID = reader.IsDBNull(reader.GetOrdinal("EmployeeID"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                OrderDate = reader.IsDBNull(reader.GetOrdinal("OrderDate"))
                    ? null
                    : reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                RequiredDate = reader.IsDBNull(reader.GetOrdinal("RequiredDate"))
                    ? null
                    : reader.GetDateTime(reader.GetOrdinal("RequiredDate")),
                ShippedDate = reader.IsDBNull(reader.GetOrdinal("ShippedDate"))
                    ? null
                    : reader.GetDateTime(reader.GetOrdinal("ShippedDate")),
                ShipVia = reader.IsDBNull(reader.GetOrdinal("ShipVia"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("ShipVia")),
                Freight = reader.IsDBNull(reader.GetOrdinal("Freight"))
                    ? null
                    : reader.GetDecimal(reader.GetOrdinal("Freight")),
                ShipName = reader.IsDBNull(reader.GetOrdinal("ShipName"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("ShipName")),
                ShipAddress = reader.IsDBNull(reader.GetOrdinal("ShipAddress"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("ShipAddress")),
                ShipCity = reader.IsDBNull(reader.GetOrdinal("ShipCity"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("ShipCity")),
                ShipRegion = reader.IsDBNull(reader.GetOrdinal("ShipRegion"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("ShipRegion")),
                ShipPostalCode = reader.IsDBNull(reader.GetOrdinal("ShipPostalCode"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("ShipPostalCode")),
                ShipCountry = reader.IsDBNull(reader.GetOrdinal("ShipCountry"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("ShipCountry"))
            };
        }

        private static OrderDetail MapOrderDetail(SqliteDataReader reader)
        {
            return new OrderDetail
            {
                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                Quantity = reader.GetInt16(reader.GetOrdinal("Quantity")),
                Discount = reader.GetFloat(reader.GetOrdinal("Discount"))
            };
        }
    }
}
