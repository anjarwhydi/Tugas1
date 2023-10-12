using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
