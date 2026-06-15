using Microsoft.AspNetCore.Mvc;

namespace WorkFlowHub.Api.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
