using GraphQL.Types;
using LocalRepository.DTO;

namespace GraphqlService.Types;

public sealed class CustomerInputType : InputObjectGraphType<Customer>
{
    public CustomerInputType()
    {
        Name = "CustomerInput";
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

public sealed class OrderInputType : InputObjectGraphType<Order>
{
    public OrderInputType()
    {
        Name = "OrderInput";
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