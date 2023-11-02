using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tugas.Models;
using Tugas.ViewModels;
using Tugas.Repository.Interface;
using System;
using Tugas.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Tugas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository repository;

        public EmployeesController(IEmployeeRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("Employees")]
        public IActionResult Get()
        {
            var employees = repository.Get();
            if (employees == null)
            {
                return StatusCode(400, new { status = HttpStatusCode.NotFound, message = "Data not found." });
            }
            return StatusCode(200, new { status = HttpStatusCode.OK, message = employees.Count() + " Data Ditemukan", data = employees });
        }

        [HttpPost("Pagging")]
        public IActionResult GetEmployees()
        {
            try
            {
                int totalRecord = 0;
                int filterRecord = 0;
                var draw = Request.Form["draw"].FirstOrDefault();
                var sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var columnName = Request.Form[$"columns[{sortColumnIndex}][name]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "10"); // Jumlah item per halaman
                int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0"); // Halaman saat ini
                var data = repository.Get().AsQueryable();

                // Jumlah total data dalam tabel
                totalRecord = data.Count();

                // Mencari data jika ada nilai pencarian
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(x => x.FirstName.ToLower().Contains(searchValue.ToLower())
                                        || x.LastName.ToLower().Contains(searchValue.ToLower())
                                        || x.Email.ToLower().Contains(searchValue.ToLower())
                                        || x.PhoneNumber.ToString().Contains(searchValue)
                                        || x.Address.ToLower().Contains(searchValue.ToLower())
                                        || x.IsActive.ToString().ToLower().Contains(searchValue.ToLower())
                                        || x.DepartName.ToLower().Contains(searchValue.ToLower()));
                }

                // Jumlah total data setelah pencarian
                filterRecord = data.Count();

                // Pengaturan pengurutan
                if (!string.IsNullOrEmpty(sortColumnIndex) && !string.IsNullOrEmpty(sortColumnDirection) && !string.IsNullOrEmpty(columnName))
                {
                    // Memeriksa arah pengurutan
                    if (sortColumnDirection == "asc")
                    {
                        data = data.OrderBy(x => x.GetType().GetProperty(columnName).GetValue(x, null));
                    }
                    else
                    {
                        data = data.OrderByDescending(x => x.GetType().GetProperty(columnName).GetValue(x, null));
                    }
                }


                // Paging
                var empList = data.Skip(skip).Take(pageSize).ToList();
                var returnObj = new
                {
                    draw = draw,
                    recordsTotal = totalRecord,
                    recordsFiltered = filterRecord,
                    data = empList
                };
                return Ok(returnObj);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet("{NIK}")]
        public IActionResult Get(string NIK)
        {
            var employee = repository.Get(NIK);
            if (employee == null)
            {
                return StatusCode(400, new { status = HttpStatusCode.NotFound, message = "Data not found." });
            }
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data has been found!", data = employee });
        }

        [HttpPut("Employee")]
        public IActionResult Update(Employee employee)
        {
            var result = repository.Update(employee);
            if (result == 0)
            {
                return StatusCode(400, new { status = HttpStatusCode.NotFound, message = "Data not found." });
            }
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data has been updated!", data = employee });
        }

        [HttpPost("Employee")]
        public IActionResult Insert(EmployeeVM employee)
        {
            var result = repository.Insert(employee);
            if (result == 0)
            {
                return StatusCode(409, new { status = HttpStatusCode.Conflict, message = "Phone number already exists in the database." });
            }
            return StatusCode(201, new { status = HttpStatusCode.Created, message = "Data has been inserted!", data = employee });
        }

        [HttpDelete("{NIK}")]
        public IActionResult Delete(string NIK)
        {
            var employee = repository.Get(NIK);
            if (employee == null)
            {
                return StatusCode(400, new { status = HttpStatusCode.NotFound, message = "Data not found." });
            }

            var result = repository.Delete(NIK);
            if (result == 0)
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Failed to delete data." });
            }
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data has been deleted!", data = employee });
        }

    }
}
