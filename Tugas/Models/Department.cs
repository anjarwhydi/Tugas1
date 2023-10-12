using System.ComponentModel.DataAnnotations;
namespace Tugas.Models
{
    public class Department
    {
        [Key]
        public string DeptID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
