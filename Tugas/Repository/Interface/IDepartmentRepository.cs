using Tugas.Models;
using Tugas.ViewModels;

namespace Tugas.Repository.Interface
{
    public interface IDepartmentRepository
    {
        IEnumerable<DepartmentVM> Get();
        DepartmentVM Get(string ID);
        int Insert(DepartmentVM department);
        int Update(DepartmentVM department);
        int Delete(string ID);
    }
}
