using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace WebAPIDemo.Controllers
{
    public class Employee1Controller : ApiController
    {

        List<Employee1> employee1s = new List<Employee1>()
        {
            new Employee1{Id = 101, Name="eswar Reddy", Age=30, City="Hyd", State="AP"},
            new Employee1{Id = 102, Name="hemani Reddy", Age=2, City="Hyd", State="AP"}
        };
        [System.Web.Http.Route("api/v1/employee")]
        public IEnumerable<Employee1> Get()
        {
            return employee1s;
        }

        [System.Web.Http.Route("api/v1/employee/{Id}")]
        public Employee1 Get(int Id)
        {
            return employee1s.FirstOrDefault(x => x.Id == Id);
        }
        public class Employee1
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public string City { get; set; }
            public string State { get; set; }

        }
    }       // GET: Employee1


}