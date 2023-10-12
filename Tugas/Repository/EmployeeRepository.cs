﻿using Tugas.Models;
using Tugas.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Tugas.Context;
using System.Globalization;
using Microsoft.Win32;
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
                throw new ArgumentException("Data not found.");
            }
            employee.IsActive = false;
            return context.SaveChanges();
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
                throw new ArgumentException("Phone number already exists in the database.");
            }
            if (!IsEmailUnique(employee.Email))
            {
                employee.Email = GenEmailForDuplicate(employee.FirstName, employee.LastName);
            }
            employee.Email = GenEmail(employee.FirstName, employee.LastName);
            var newEmployee = new Employee
            {
                NIK = GenNIK(),
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                IsActive = true,
                Department_ID = employee.DepartID
            };
            context.Employees.Add(newEmployee);
            return context.SaveChanges();
        }

        public int Update(Employee employee)
        {
            var existingEmployee = context.Employees.FirstOrDefault(e => e.NIK == employee.NIK);
            if (existingEmployee == null)
            {
                throw new ArgumentException("Data not found.");
            }

            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.PhoneNumber = employee.PhoneNumber;
            existingEmployee.Address = employee.Address;
            existingEmployee.Department_ID = employee.Department_ID;

            context.Entry(existingEmployee).State = EntityState.Modified;
            return context.SaveChanges();
        }

        public string GenEmail(string First, string Last)
        {
            string newNIK = $"{First}.{Last}@berca.co.id";
            return newNIK;
        }

        public string GenEmailForDuplicate(string First, string Last)
        {
            int existingCount = context.Employees.Count(e => e.Email.StartsWith($"{First}.{Last}"));
            string newNIK = $"{First}.{Last}{existingCount + 1:D3}@berca.co.id";
            return newNIK;
        }

        public string GenNIK()
        {
            string currentDate = DateTime.Now.ToString("ddMMyy");

            int existingCount = context.Employees.Count(e => e.NIK.StartsWith(currentDate));

            string newNIK = $"{currentDate}{existingCount + 1:D3}";

            return newNIK;
        }

        public bool IsEmailUnique(string email)
        {
            return !context.Employees.Any(e => e.Email == email);
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
                              Nama = e.FirstName + " " + e.LastName,
                              Email = e.Email,
                              PhoneNumber = e.PhoneNumber,
                              Address = e.Address,
                              Department = d.Name
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
                              Nama = e.FirstName + " " + e.LastName,
                              Email = e.Email,
                              PhoneNumber = e.PhoneNumber,
                              Address = e.Address,
                              Department = d.Name
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
            var result = context.Employees.Where(e => e.IsActive == true).GroupBy(e => e.Department.Name).Select(g => new TotalActiveEmp
            {
                department = g.Key,
                total = g.Count()
            }).ToList();
            return result;
        }
    }
}
