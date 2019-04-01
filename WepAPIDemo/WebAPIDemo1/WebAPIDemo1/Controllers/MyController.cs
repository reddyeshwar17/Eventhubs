using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace WebAPIDemo1.Controllers
{
    public class MyController : Controller
    {
        // GET: My
        [WebMethod]
        public string Index(string name)
        {
            return "Hello " + name + Environment.NewLine + "The Current Time is: "
                + DateTime.Now.ToString();
        }
    }
}