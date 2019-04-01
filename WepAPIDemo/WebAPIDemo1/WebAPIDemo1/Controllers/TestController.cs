using System.Web.Mvc;

namespace WebAPIDemo1.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public string Index()
        {
            return "First Nane:" + Request.Form["fName"] + " |Last Name" + Request.Form["lName"];
        }
    }
}