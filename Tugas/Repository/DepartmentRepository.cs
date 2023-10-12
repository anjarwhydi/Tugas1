using Microsoft.EntityFrameworkCore;
using Tugas.Models;
using Tugas.Repository.Interface;
using Tugas.Context;
using Tugas.ViewModels;

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

        public IEnumerable<DepartmentVM> Get()
        {
            var result = (from d in context.Departments
                          select new DepartmentVM
                          {
                              DeptID = d.DeptID,
                              Name = d.Name
                          }).ToList();
            return result;
        }

        public DepartmentVM Get(string ID)
        {
            var result = (from d in context.Departments
                          where d.DeptID == ID
                          select new DepartmentVM
                          {
                              DeptID = d.DeptID,
                              Name = d.Name
                          }).FirstOrDefault();
            return result;
        }

        public int Insert(DepartmentVM department)
        {
            var newDepartment = new Department
            {
                DeptID = GenDeptID(),
                Name = department.Name
            };
            context.Departments.Add(newDepartment);
            var results = context.SaveChanges();
            return results;
        }

        public int Update(DepartmentVM department)
        {
            var existingDepartment = context.Departments.FirstOrDefault(e => e.DeptID == department.DeptID);
            if (existingDepartment == null)
            {
                throw new ArgumentException("Data not found.");
            }

            existingDepartment.Name = department.Name;
            return context.SaveChanges();
        }

        //public string GenDeptID()
        //{
        //    int existingCount = context.Departments.Count();

        //    string newID = $"D{existingCount + 1:D3}";

        //    return newID;
        //}

        //public string GenDeptID()
        //{
        //    int existingCount = context.Departments.Count();
        //    int newID;

        //    if (existingCount == 0)
        //    {
        //        newID = 1;
        //    }
        //    else
        //    {
        //        string maxID = context.Departments.Max(d => d.DeptID);
        //        newID = int.Parse(maxID.Substring(1)) + 1;
        //    }

        //    return $"D{newID:D3}";
        //}

        public string GenDeptID()
        {
            var existingIDs = context.Departments.Select(d => d.DeptID).ToList();

            for (int i = 1; ; i++)
            {
                string newID = $"D{i:D3}";

                if (!existingIDs.Contains(newID))
                {
                    return newID;
                }
            }
        }


    }
}
