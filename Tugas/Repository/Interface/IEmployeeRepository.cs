using Tugas.Models;
using Tugas.ViewModels;

namespace Tugas.Repository.Interface
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> Get();
        Employee Get(string NIK);
        int Insert(Employee employee);
        int Update(Employee employee);
        int Delete(string NIK);
        List<GetActiveEmpDept> GetActiveEmpDept();
        List<GetNonActiveEmpDept> GetNonActiveEmpDept();
        List<GetActiveEmpPerDept> GetActiveEmpPerDept(string depart);
        List<GetNonActiveEmpPerDept> GetNonActiveEmpPerDept(string depart);

    }
}
