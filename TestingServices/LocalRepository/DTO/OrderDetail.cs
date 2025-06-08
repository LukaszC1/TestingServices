using System.Runtime.Serialization;

namespace LocalRepository.DTO
{
    [DataContract(Namespace = "http://tempuri.org/")]
    public class OrderDetail
    {
        [DataMember] 
        public int OrderID { get; set; }

        [DataMember]
        public int ProductID { get; set; }

        [DataMember] 
        public decimal UnitPrice { get; set; }

        [DataMember]
        public short Quantity { get; set; }

        [DataMember] 
        public float Discount { get; set; }
    }
}
