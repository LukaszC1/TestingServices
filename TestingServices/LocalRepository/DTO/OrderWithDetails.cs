using System.Runtime.Serialization;

namespace LocalRepository.DTO
{
    [DataContract(Namespace = "http://tempuri.org/")]
    public class OrderWithDetails
    {
        [DataMember] 
        public int OrderID { get; set; }

        [DataMember] 
        public string CustomerID { get; set; }

        [DataMember]
        public int EmployeeID { get; set; }

        [DataMember] 
        public DateTime OrderDate { get; set; }

        [DataMember] 
        public List<OrderDetail> OrderDetails { get; set; } = [];
    }
}