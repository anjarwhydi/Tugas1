using Tugas.Models;
using Tugas.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Tugas.Context;
using Tugas.ViewModels;

namespace Tugas.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MyContext context;

        public EmployeeRepository(MyContext context)
        {
            this.context = context;
        }

        public int Delete(string NIK)
        {
            var employee = context.Employees.Find(NIK);
            if (employee == null)
            {
                return 0; // Ganti dengan 0 sebagai tanda bahwa data tidak ditemukan
            }
            employee.IsActive = false;
            context.SaveChanges();
            return 1; // Ganti dengan 1 sebagai tanda bahwa data telah dihapus
        }

        public IEnumerable<GetEmployeeVM> Get()
        {
            var result = (from e in context.Employees
                          join d in context.Departments on e.Department_ID equals d.DeptID
                          select new GetEmployeeVM
                          {
                              NIK = e.NIK,
                              FirstName = e.FirstName,
                              LastName = e.LastName,
                              PhoneNumber = e.PhoneNumber,
                              Email = e.Email,
                              Address = e.Address,
                              IsActive = e.IsActive,
                              DepartName = d.Name
                          }).ToList();

            return result;
        }

        public Employee Get(string NIK)
        {
            return context.Employees.FirstOrDefault(e => e.NIK == NIK);
        }

        public int Insert(EmployeeVM employee)
        {
            if (!IsPhoneUnique(employee.PhoneNumber))
            {
                return 0; // Ganti dengan 0 sebagai tanda bahwa nomor telepon sudah ada
            }
            var newEmployee = new Employee
            {
                NIK = GenNIK(),
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = GeneratedEmail(employee.FirstName, employee.LastName),
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                IsActive = true,
                Department_ID = employee.DepartID
            };
            context.Employees.Add(newEmployee);
            context.SaveChanges();
            return 1; // Ganti dengan 1 sebagai tanda bahwa data telah dimasukkan
        }

        public int Update(Employee employee)
        {
            var existingEmployee = context.Employees.FirstOrDefault(e => e.NIK == employee.NIK);
            if (existingEmployee == null)
            {
                return 0; // Ganti dengan 0 sebagai tanda bahwa data tidak ditemukan
            }

            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.PhoneNumber = employee.PhoneNumber;
            existingEmployee.Address = employee.Address;
            existingEmployee.Department_ID = employee.Department_ID;

            context.Entry(existingEmployee).State = EntityState.Modified;
            context.SaveChanges();
            return 1; // Ganti dengan 1 sebagai tanda bahwa data telah diperbarui
        }

        public string GenNIK()
        {
            string currentDate = DateTime.Now.ToString("ddMMyy");

            int existingCount = context.Employees.Count(e => e.NIK.StartsWith(currentDate));

            string newNIK = $"{currentDate}{existingCount + 1:D3}";

            return newNIK;
        }

        private string GeneratedEmail(string FirstName, string LastName)
        {
            string baseEmail = $"{FirstName.ToLower()}.{LastName.ToLower()}";
            string generatedEmail = $"{baseEmail}{"@berca.co.id"}";
            int counter = 1;

            while (context.Employees.Any(u => u.Email == generatedEmail))
            {
                generatedEmail = $"{baseEmail}{counter:D3}{"@berca.co.id"}";
                counter++;

                if (counter > 999)
                {
                    throw new Exception("Unable to generate a unique username.");
                }
            }

            return generatedEmail;
        }

        public bool IsPhoneUnique(string phone)
        {
            return !context.Employees.Any(e => e.PhoneNumber == phone);
        }

        public List<GetActiveEmpDept> GetActiveEmpDept()
        {
            var result = (from e in context.Employees
                          join d in context.Departments on e.Department_ID equals d.DeptID
                          where e.IsActive == true
                          select new GetActiveEmpDept
                          {
                              NIK = e.NIK,
                              FirstName = e.FirstName,
                              LastName = e.LastName,
                              PhoneNumber = e.PhoneNumber,
                              Email = e.Email,
                              Address = e.Address,
                              IsActive = e.IsActive,
                              DepartName = d.Name
                          }).ToList();
            return result;
        }

        public List<GetNonActiveEmpDept> GetNonActiveEmpDept()
        {
            var result = (from e in context.Employees
                          join d in context.Departments on e.Department_ID equals d.DeptID
                          where e.IsActive == false
                          select new GetNonActiveEmpDept
                          {
                              NIK = e.NIK,
                              FirstName = e.FirstName,
                              LastName = e.LastName,
                              PhoneNumber = e.PhoneNumber,
                              Email = e.Email,
                              Address = e.Address,
                              IsActive = e.IsActive,
                              DepartName = d.Name
                          }).ToList();
            return result;
        }

        public List<GetActiveEmpPerDept> GetActiveEmpPerDept(string depart)
        {
            var result = (from e in context.Employees
                          join d in context.Departments on e.Department_ID equals d.DeptID
                          where e.Department_ID == depart && e.IsActive == true
                          select new GetActiveEmpPerDept
                          {
                              Nama = e.FirstName + " " + e.LastName,
                              Email = e.Email,
                              PhoneNumber = e.PhoneNumber,
                              Address = e.Address,
                          }).ToList();
            return result;
        }

        public List<GetNonActiveEmpPerDept> GetNonActiveEmpPerDept(string depart)
        {
            var result = (from e in context.Employees
                          join d in context.Departments on e.Department_ID equals d.DeptID
                          where e.Department_ID == depart && e.IsActive == false
                          select new GetNonActiveEmpPerDept
                          {
                              Nama = e.FirstName + " " + e.LastName,
                              Email = e.Email,
                              PhoneNumber = e.PhoneNumber,
                              Address = e.Address,
                          }).ToList();
            return result;
        }

        public List<TotalActiveEmp> TotalActiveEmp()
        {
            var result = context.Employees.Where(e => e.IsActive == true).GroupBy(e => e.Department.Name).Select(g => new TotalActiveEmp
            {
                department = g.Key,
                total = g.Count()
            }).ToList();
            return result;
        }

        public List<TotalActiveEmp> TotalNonActiveEmp()
        {
            var result = context.Employees.Where(e => e.IsActive == false).GroupBy(e => e.Department.Name).Select(g => new TotalActiveEmp
            {
                department = g.Key,
                total = g.Count()
            }).ToList();
            return result;
        }
    }
}
