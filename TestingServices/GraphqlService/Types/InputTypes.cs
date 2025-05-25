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
    }
}