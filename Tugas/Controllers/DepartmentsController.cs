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

        [HttpGet("Department")]
        public ActionResult Get()
        {
            try
            {
                var departments = repository.Get();
                return CreateResponse(HttpStatusCode.OK, "Data has been found!", departments);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpGet("{ID}")]
        public ActionResult Get(string ID)
        {
            try
            {
                var department = repository.Get(ID);
                if (department == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, "Data not found.");
                }
                return CreateResponse(HttpStatusCode.OK, "Data has been found!", department);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpPut("Department")]
        public ActionResult Update(DepartmentVM department)
        {
            try
            {
                repository.Update(department);
                return CreateResponse(HttpStatusCode.OK, "Data has been updated!", department);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpPost("Department")]
        public ActionResult Insert(DepartmentVM department)
        {
            try
            {
                repository.Insert(department);
                return CreateResponse(HttpStatusCode.Created, "Data has been inserted!", department);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.Conflict, ex.Message);
            }
        }

        [HttpDelete("{ID}")]
        public ActionResult Delete(string ID)
        {
            try
            {
                var department = repository.Get(ID);
                if (department == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, "Data not found.");
                }
                repository.Delete(ID);
                return CreateResponse(HttpStatusCode.OK, "Data has been deleted!", department);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
