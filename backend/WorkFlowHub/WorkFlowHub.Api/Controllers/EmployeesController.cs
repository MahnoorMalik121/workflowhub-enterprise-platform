using Microsoft.AspNetCore.Mvc;

namespace WorkFlowHub.Api.Controllers
{
    public class EmployeesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
