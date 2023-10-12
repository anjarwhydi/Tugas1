using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class EmployeesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
