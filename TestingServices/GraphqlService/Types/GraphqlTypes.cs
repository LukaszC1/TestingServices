using GraphQL.Types;
using LocalRepository.DTO;

public sealed class ProductType : ObjectGraphType<Product>
{
    public ProductType()
    {
        Field(x => x.ProductID);
        Field(x => x.ProductName, nullable: true);
        Field(x => x.SupplierID, nullable: true);
        Field(x => x.CategoryID, nullable: true);
        Field(x => x.QuantityPerUnit, nullable: true);
        Field(x => x.UnitPrice, nullable: true);
        Field(x => x.UnitsInStock, nullable: true);
        Field(x => x.UnitsOnOrder, nullable: true);
        Field(x => x.ReorderLevel, nullable: true);
        Field(x => x.Discontinued);
    }
}

public sealed class CustomerType : ObjectGraphType<Customer>
{
    public CustomerType()
    {
        Field(x => x.CustomerID, nullable: true);
        Field(x => x.CompanyName, nullable: true);
        Field(x => x.ContactName, nullable: true);
        Field(x => x.ContactTitle, nullable: true);
        Field(x => x.Address, nullable: true);
        Field(x => x.City, nullable: true);
        Field(x => x.Region, nullable: true);
        Field(x => x.PostalCode, nullable: true);
        Field(x => x.Country, nullable: true);
        Field(x => x.Phone, nullable: true);
        Field(x => x.Fax, nullable: true);
    }
}

public sealed class OrderType : ObjectGraphType<Order>
{
    public OrderType()
    {
        Field(x => x.OrderID);
        Field(x => x.CustomerID, nullable: true);
        Field(x => x.EmployeeID, nullable: true);
        Field(x => x.OrderDate, nullable: true);
        Field(x => x.RequiredDate, nullable: true);
        Field(x => x.ShippedDate, nullable: true);
        Field(x => x.ShipVia, nullable: true);
        Field(x => x.Freight, nullable: true);
        Field(x => x.ShipName, nullable: true);
        Field(x => x.ShipAddress, nullable: true);
        Field(x => x.ShipCity, nullable: true);
        Field(x => x.ShipRegion, nullable: true);
        Field(x => x.ShipPostalCode, nullable: true);
        Field(x => x.ShipCountry, nullable: true);
    }
}

public sealed class OrderDetailType : ObjectGraphType<OrderDetail>
{
    public OrderDetailType()
    {
        Field(x => x.OrderID);
        Field(x => x.ProductID);
        Field(x => x.UnitPrice);
        Field(x => x.Quantity);
        Field(x => x.Discount);
    }
}
