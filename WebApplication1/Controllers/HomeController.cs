using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Api;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        public IConfig _config;
        public HomeController(IConfig config) 
        {
            _config = config;
        }


        [HttpGet]
        public ActionResult Index()
        {

            _config.ww();
            return View();
        }
    }
}
