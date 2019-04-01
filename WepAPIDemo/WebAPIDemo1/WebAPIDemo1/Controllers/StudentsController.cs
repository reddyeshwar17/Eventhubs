using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using WebAPIDemo1.Models;

namespace WebAPIDemo1.Controllers
{
    public class StudentsController : ApiController
    {
        private StudentContext db = new StudentContext();

        // GET api/<controller>
        public IHttpActionResult GetAllStudents()
        {
            IList<StandardViewModel> students = null;

            //var student = db.Students.ToList();

            //return Ok(student);
            var list = (from a in db.Students
                        join f in db.Student on a.StandardId equals f.Id into tmpstud
                        join g in db.Address on a.StandardId equals g.StudentId into tmpaddr

                        from f in tmpstud.DefaultIfEmpty()
                        from g in tmpaddr.DefaultIfEmpty()
                        select new
                        {
                            a.StandardId,
                            a.Name,
                            f.LastName,
                            f.FirstName,
                            g.Address1,
                            g.Address2,
                            g.State,
                            g.City
                        });

            var q2 = (from a in db.Students
                      from b in db.Student.Where(b => b.Id == a.StandardId).DefaultIfEmpty()
                      from f in db.Address.Where(f => f.StudentId == a.StandardId).DefaultIfEmpty()
                      select new
                      {
                          a.StandardId,
                          a.Name,
                          b.FirstName,
                          b.LastName,
                          f.Address1,
                          f.Address2,
                          f.City,
                          f.State
                      }
                    );

            return Ok(list);

            /* var results = from item in db.Students
                           select new
                           {
                               item.Name,
                               item.StandardId,
                               item.Students = from student in db.Student
                                               where student.Id == item.StandardId)
                                               select new
                                               {
                                                   student.Id,
                                                   student.FirstName,
                                                   student.LastName,
                                                   student.Address = from addr in db.Address
                                                                     where addr.StudentId == student.Id
                                                                     select new
                                                                     {
                                                                         addr.Address1,
                                                                         addr.Address2,
                                                                         addr.City,
                                                                         addr.State
                                                                     }
                                               };*/
        }

        // GET api/<controller>/5
        public IHttpActionResult GetStudentbyId(int id)
        {
            var list = from a in db.Students
                       where (id == a.StandardId)
                       join b in db.Student on id equals b.Id
                       join c in db.Address on id equals c.StudentId
                       select new
                       {
                           a.StandardId,
                           a.Name,
                           b.FirstName,
                           b.LastName,
                           c.Address1,
                           c.Address2,
                           c.City,
                           c.State
                       };

            if (list == null)
                return NotFound();
            return Ok(list);
        }

        // POST api/<controller>
        public IHttpActionResult PostNewStudent(StandardViewModel student)
        {
            var context = new StudentContext();
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");
            /* var ctx = new StandardViewModel
             {
                Name="Eswar Reddy Deva",
                StandardId=101,

                  Students = new List<StudentViewModel>()
                  {
                      new StudentViewModel
                    {
                      FirstName = "Deva",
                      LastName = "Reddy",
                      Id = 101,
                      Address = new AddressViewModel()
                      {
                          Address1 = "Hyderabad2",
                          Address2 = "CMG Palli2",
                          City = "Kadapa2",
                          State = "AP2",
                          StudentId = 101
                      }
                     }
                  }
             };*/

            context.Students.Add(student);
            context.SaveChanges();
            return Ok();
        }

        // PUT api/<controller>/5
        [HttpPut]
        public IHttpActionResult PutStudent(StudentViewModel student)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");
            var existingStudent = db.Student.Where(s => s.Id == student.Id).FirstOrDefault();
            if (existingStudent != null)
            {
                existingStudent.FirstName = student.FirstName;
                existingStudent.LastName = student.LastName;
            }
            db.SaveChanges();
            return Ok();
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            var studentToDelete = db.Students.Where(s => s.StandardId == id).FirstOrDefault();

            db.Entry(studentToDelete).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, studentToDelete);
        }
    }
}