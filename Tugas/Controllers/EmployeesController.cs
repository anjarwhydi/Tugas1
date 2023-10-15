using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tugas.Models;
using Tugas.ViewModels;
using Tugas.Repository.Interface;
using System;

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
