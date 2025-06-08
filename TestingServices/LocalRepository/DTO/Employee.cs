using System.Runtime.Serialization;

namespace LocalRepository.DTO
{
    [DataContract(Namespace = "http://tempuri.org/")]
    public class Employee
    {
        [DataMember] 
        public int EmployeeID { get; set; }

        [DataMember] 
        public string FirstName { get; set; } = string.Empty;

        [DataMember]
        public string LastName { get; set; } = string.Empty;
    }
}