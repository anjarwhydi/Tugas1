using Microsoft.EntityFrameworkCore;
using Tugas.Models;
using Tugas.Repository.Interface;
using Tugas.Context;
using Tugas.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

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
                return 0; // Ganti dengan 0 sebagai tanda bahwa data tidak ditemukan
            }
            context.Departments.Remove(department);
            context.SaveChanges();
            return 1; // Ganti dengan 1 sebagai tanda bahwa data telah dihapus
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
            context.SaveChanges();
            return 1; // Ganti dengan 1 sebagai tanda bahwa data telah dimasukkan
        }

        public int Update(DepartmentVM department)
        {
            var existingDepartment = context.Departments.FirstOrDefault(d => d.DeptID == department.DeptID);
            if (existingDepartment == null)
            {
                return 0; // Ganti dengan 0 sebagai tanda bahwa data tidak ditemukan
            }

            existingDepartment.Name = department.Name;
            context.SaveChanges();
            return 1; // Ganti dengan 1 sebagai tanda bahwa data telah diperbarui
        }

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
