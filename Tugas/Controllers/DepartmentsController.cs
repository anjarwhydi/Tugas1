using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tugas.Models;
using Tugas.ViewModels;
using Tugas.Repository.Interface;

namespace Tugas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentRepository repository;

        public DepartmentsController(IDepartmentRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("Department")]
        public IActionResult Get()
        {
            var departments = repository.Get();
            if (departments == null)
            {
                return StatusCode(400, new { status = HttpStatusCode.NotFound, message = "Data not found." });
            }
            return StatusCode(200, new { status = HttpStatusCode.OK, message = departments.Count() + " Data Ditemukan", data = departments });
        }

        [HttpGet("{ID}")]
        public IActionResult Get(string ID)
        {
            var department = repository.Get(ID);
            if (department == null)
            {
                return StatusCode(400, new { status = HttpStatusCode.NotFound, message = "Data not found." });
            }
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data has been found!", data = department });
        }

        [HttpPut("Department")]
        public IActionResult Update(DepartmentVM department)
        {
            var result = repository.Update(department);
            if (result == 0)
            {
                return StatusCode(400, new { status = HttpStatusCode.NotFound, message = "Data not found." });
            }
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data has been updated!", data = department });
        }

        [HttpPost("Department")]
        public IActionResult Insert(DepartmentVM department)
        {
            var result = repository.Insert(department);
            if (result == 0)
            {
                return StatusCode(409, new { status = HttpStatusCode.Conflict, message = "Failed to insert data." });
            }
            return StatusCode(201, new { status = HttpStatusCode.Created, message = "Data has been inserted!", data = department });
        }

        [HttpDelete("{ID}")]
        public IActionResult Delete(string ID)
        {
            var department = repository.Get(ID);
            if (department == null)
            {
                return StatusCode(400, new { status = HttpStatusCode.NotFound, message = "Data not found." });
            }

            var result = repository.Delete(ID);
            if (result == 0)
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Failed to delete data." });
            }
            return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data has been deleted!", data = department });
        }
    }
}
