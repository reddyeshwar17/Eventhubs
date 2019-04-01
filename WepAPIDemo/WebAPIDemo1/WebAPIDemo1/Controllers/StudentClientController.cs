using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using WebAPIDemo1.Models;

namespace WebAPIDemo1.Controllers
{
    public class StudentClientController : Controller
    {
        // GET: StudentClient
        public ActionResult Index()
        {
            IEnumerable<StudentViewModel> students = null;

            string userName = Environment.UserName;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://localhost:65104/api/");
                var response = httpClient.GetAsync("students");
                response.Wait();
                var result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<StudentViewModel>>();
                    readTask.Wait();

                    students = readTask.Result;
                }
            }

            return View(students);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(StudentViewModel student)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:65104/api/students");

                var postTask = client.PostAsJsonAsync<StudentViewModel>("students", student);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(student);
        }

        public ActionResult Edit(int id)
        {
            List<StudentViewModel> students = null;
            StudentViewModel student = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:65104/api/");
                var getTask = client.GetAsync("students?id=" + id.ToString());
                getTask.Wait();
                var result = getTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<StudentViewModel>>();

                    //readTask = JsonConvert.DeserializeObject(readTask);
                    readTask.Wait();
                    students = readTask.Result;
                    if (students.Count > 0)
                        student = students[0];
                }
            }
            return View(student);
        }

        [HttpPost]
        public ActionResult Edit(StudentViewModel student)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:65104/api/");

                var getTask = client.PostAsJsonAsync<StudentViewModel>("Students", student);
                getTask.Wait();

                var result = getTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(student);
        }
    }
}