using System.Runtime.Serialization;

namespace LocalRepository.DTO
{
    [DataContract(Namespace = "http://tempuri.org/")]
    public class Product
    {
        [DataMember] 
        public int ProductID { get; set; }

        [DataMember] 
        public string? ProductName { get; set; }

        [DataMember] 
        public int? SupplierID { get; set; }

        [DataMember]
        public int? CategoryID { get; set; }

        [DataMember]
        public string? QuantityPerUnit { get; set; }

        [DataMember] 
        public decimal? UnitPrice { get; set; }

        [DataMember] 
        public short? UnitsInStock { get; set; }

        [DataMember]
        public short? UnitsOnOrder { get; set; }

        [DataMember] 
        public short? ReorderLevel { get; set; }

        [DataMember]
        public bool Discontinued { get; set; }
    }
}
