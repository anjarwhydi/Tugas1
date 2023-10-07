using System.ComponentModel.DataAnnotations;

namespace Tugas.Models
{
    public class Employee
    {
        [Key]
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public string Department_ID { get; set; }
        public Department Department { get; set; }
    }
}
