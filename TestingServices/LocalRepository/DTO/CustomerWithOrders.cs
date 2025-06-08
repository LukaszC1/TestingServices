using System.Runtime.Serialization;

namespace LocalRepository.DTO
{
    [DataContract(Namespace = "http://tempuri.org/")]
    public class CustomerWithOrders
    {
        [DataMember] 
        public string CustomerID { get; set; }

        [DataMember] 
        public string CompanyName { get; set; }

        [DataMember] 
        public string ContactName { get; set; }

        [DataMember] 
        public List<OrderWithDetails> Orders { get; set; } = [];
    }
}