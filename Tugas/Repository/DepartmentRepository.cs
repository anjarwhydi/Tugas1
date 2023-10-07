using Microsoft.EntityFrameworkCore;
using Tugas.Models;
using Tugas.Repository.Interface;
using Tugas.Context;

namespace Tugas.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly MyContext context;

        public DepartmentRepository(MyContext context)
        {
            this.context = context;
        }

        public int Delete(string ID)
        {
            var department = context.Departments.Find(ID);
            if (department == null)
            {
                throw new ArgumentException("Data not found.");
            }
            context.Departments.Remove(department);
            return context.SaveChanges();
        }

        public IEnumerable<Department> Get()
        {
            return context.Departments.ToList();
        }

        public Department Get(string ID)
        {
            return context.Departments.FirstOrDefault(d => d.DeptID == ID);
        }

        public int Insert(Department department)
        {
            context.Departments.Add(department);
            department.DeptID = GenDeptID();
            var results = context.SaveChanges();
            return results;
        }

        public int Update(Department department)
        {
            var existingDepartment = context.Departments.FirstOrDefault(e => e.DeptID == department.DeptID);
            if (existingDepartment == null)
            {
                throw new ArgumentException("Data not found.");
            }

            existingDepartment.Name = department.Name;

            context.Entry(existingDepartment).State = EntityState.Modified;
            return context.SaveChanges();
        }

        public string GenDeptID()
        {
            int existingCount = context.Departments.Count(d => d.DeptID.StartsWith("D"));

            string newID = $"{existingCount + 1:D3}";

            return newID;
        }
    }
}
