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
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository repository;

        public EmployeesController(IEmployeeRepository repository)
        {
            this.repository = repository;
        }

        private ActionResult CreateResponse(HttpStatusCode statusCode, string message, object data = null)
        {
            if (data == null)
            {
                var responseDataNull = new JsonResult(new
                {
                    status_code = (int)statusCode,
                    message,
                });

                return responseDataNull;

            }

            var response = new JsonResult(new
            {
                status_code = (int)statusCode,
                message,
                data
            });

            return response;
        }

        [HttpGet("Employees")]
        public ActionResult Get()
        {
            try
            {
                var employees = repository.Get();
                return CreateResponse(HttpStatusCode.OK, "Data has been found!", employees);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpGet("{NIK}")]
        public ActionResult Get(string NIK)
        {
            try
            {
                var employee = repository.Get(NIK);
                if (employee == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, "Data not found.");
                }
                return CreateResponse(HttpStatusCode.OK, "Data has been found!", employee);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpPut("Employee")]
        public ActionResult Update(Employee employee)
        {
            try
            {
                repository.Update(employee);
                return CreateResponse(HttpStatusCode.OK, "Data has been updated!", employee);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpPost("Employee")]
        public ActionResult Insert(EmployeeVM employee)
        {
            try
            {
                repository.Insert(employee);
                return CreateResponse(HttpStatusCode.Created, "Data has been inserted!", employee);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.Conflict, ex.Message);
            }
        }

        [HttpDelete("{NIK}")]
        public ActionResult Delete(string NIK)
        {
            try
            {
                var employee = repository.Get(NIK);
                if (employee == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, "Data not found.");
                }
                repository.Delete(NIK);
                return CreateResponse(HttpStatusCode.OK, "Data has been deleted!", employee);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet("GetActiveEmpDept")]
        public ActionResult GetActiveEmpDept()
        {
            try
            {
                var employee = repository.GetActiveEmpDept();
                if (employee == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, "Data not found.");
                }
                return CreateResponse(HttpStatusCode.OK, "Data has been found!", employee);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpGet("GetNonActiveEmpDept")]
        public ActionResult GetNonActiveEmpDept()
        {
            try
            {
                var employee = repository.GetNonActiveEmpDept();
                if (employee == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, "Data not found.");
                }
                return CreateResponse(HttpStatusCode.OK, "Data has been found!", employee);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpGet("GetActiveEmpPerDept")]
        public ActionResult GetActiveEmpPerDept(string depart)
        {
            try
            {
                var employee = repository.GetActiveEmpPerDept(depart);
                if (employee == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, "Data not found.");
                }
                return CreateResponse(HttpStatusCode.OK, "Data has been found!", employee);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpGet("GetNonActiveEmpPerDept")]
        public ActionResult GetNonActiveEmpPerDept(string depart)
        {
            try
            {
                var employee = repository.GetNonActiveEmpPerDept(depart);
                if (employee == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, "Data not found.");
                }
                return CreateResponse(HttpStatusCode.OK, "Data has been found!", employee);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpGet("TotalActiveEmp")]
        public ActionResult TotalActiveEmp()
        {
            try
            {
                var employee = repository.TotalActiveEmp();
                if (employee == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, "Data not found.");
                }
                return CreateResponse(HttpStatusCode.OK, "Data has been found!", employee);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpGet("TotalNonActiveEmp")]
        public ActionResult TotalNonActiveEmp()
        {
            try
            {
                var employee = repository.TotalNonActiveEmp();
                if (employee == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, "Data not found.");
                }
                return CreateResponse(HttpStatusCode.OK, "Data has been found!", employee);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }
    }
}
