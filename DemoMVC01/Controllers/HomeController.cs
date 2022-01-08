using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoMVC01.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if ((exceptionHandlerPathFeature != null) && (exceptionHandlerPathFeature.Error != null))
            {
                // In our example, the ExceptionHelper.LogException() method will take care of   
                // logging the exception to the database and perhaps even alerting the webmaster  
                // Make sure that this method doesn't throw any exceptions or you might end  
                // in an endless loop!  
                //ExceptionHelper.LogException(exceptionHandlerPathFeature.Error);
            }
            return Content("We're so sorry, but an error just occurred! It has been logged and we'll try to get it fixed ASAP!");
        }
    }
        
    
}

