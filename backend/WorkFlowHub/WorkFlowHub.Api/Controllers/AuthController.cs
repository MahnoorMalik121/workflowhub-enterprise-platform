using Microsoft.AspNetCore.Mvc;

namespace WorkFlowHub.Api.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
