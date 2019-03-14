using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPIDemo.Controllers
{
    public class Employee2Controller : ApiController
    {
        List<Employee2> employee2s = new List<Employee2>()
        {
            new Employee2{Id = 101, FirstName="Eswar", LastName="Devarinti",DOB = new DateTime(1987, 06,15), City="Redmond", State="US"},
            new Employee2{Id = 101, FirstName="hEMANI", LastName="Devarinti",DOB = new DateTime(2018, 06,15), City="Redmond", State="US"},
        };

        public IEnumerable<Employee2> Get()
        {
            return employee2s;
        }

        public Employee2 Get(int Id)
        {
            return employee2s.FirstOrDefault(x => x.Id == Id);
        }
        public class Employee2
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DOB { get; set; }
            public string City { get; set; }
            public string State { get; set; }

        }
    }
}