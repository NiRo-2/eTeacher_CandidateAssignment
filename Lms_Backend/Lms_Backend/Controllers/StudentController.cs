using Microsoft.AspNetCore.Mvc;

namespace Lms_Backend.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
