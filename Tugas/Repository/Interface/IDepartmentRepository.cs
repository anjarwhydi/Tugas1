using Tugas.Models;

namespace Tugas.Repository.Interface
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> Get();
        Department Get(string ID);
        int Insert(Department department);
        int Update(Department department);
        int Delete(string ID);
    }
}
