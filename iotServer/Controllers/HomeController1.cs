using Microsoft.AspNetCore.Mvc;

namespace iotServer.Controllers
{
    public class HomeController1 : Controller
    {
        public string Index()
        {
            return "Dit is de index";
        }

        public string testFunc()
        {
            return "Hello World!";
        }
    }
}
