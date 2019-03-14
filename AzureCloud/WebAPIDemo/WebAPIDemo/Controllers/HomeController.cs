using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebAPIDemo.BasicAuthentication;

namespace WebAPIDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // ViewBag.Title = "Home Page";
            //return View();

            //string FullName = User.FirstName + " " + User.LastName;

            HttpClient client = new HttpClient();

            string authInfo = "admin" + ":" + "123456";
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);
            // http://localhost:55247/api/axmonthlydatas
            client.BaseAddress = new Uri("http://localhost:55247/");

            HttpResponseMessage response = client.GetAsync("api/axmonthlydatas/").Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                //var data = response.Content.ReadAsAsync<>().Result;
                return View();
            }
            return View();
        }
    }
}

