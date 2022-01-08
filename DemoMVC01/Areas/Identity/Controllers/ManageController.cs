using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoMVC01.Areas.Identity.Controllers
{
    public class ManageController : Controller
    {
        [Authorize]
        [Area("Identity")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
