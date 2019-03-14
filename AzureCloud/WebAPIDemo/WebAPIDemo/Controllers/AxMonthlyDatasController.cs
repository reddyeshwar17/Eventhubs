using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPIDemo.BasicAuthentication;
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{
    public class AxMonthlyDatasController : ApiController
    {
        private SQLInfoDBEntities db = new SQLInfoDBEntities();

        // GET: api/AxMonthlyDatas
        [CustomAuthorize(Roles = "Admin")]
        public IQueryable<AxMonthlyData> GetAxMonthlyDatas()
        {
            return db.AxMonthlyDatas;
        }

        // GET: api/AxMonthlyDatas/5
        [ResponseType(typeof(AxMonthlyData))]
        public IHttpActionResult GetAxMonthlyData(DateTime id)
        {
            AxMonthlyData axMonthlyData = db.AxMonthlyDatas.Find(id);
            if (axMonthlyData == null)
            {
                return NotFound();
            }

            return Ok(axMonthlyData);
        }

        // PUT: api/AxMonthlyDatas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAxMonthlyData(DateTime id, AxMonthlyData axMonthlyData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != axMonthlyData.CreatedDate)
            {
                return BadRequest();
            }

            db.Entry(axMonthlyData).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AxMonthlyDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AxMonthlyDatas
        [ResponseType(typeof(AxMonthlyData))]
        public IHttpActionResult PostAxMonthlyData(AxMonthlyData axMonthlyData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AxMonthlyDatas.Add(axMonthlyData);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AxMonthlyDataExists(axMonthlyData.CreatedDate))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = axMonthlyData.CreatedDate }, axMonthlyData);
        }

        // DELETE: api/AxMonthlyDatas/5
        [ResponseType(typeof(AxMonthlyData))]
        public IHttpActionResult DeleteAxMonthlyData(DateTime id)
        {
            AxMonthlyData axMonthlyData = db.AxMonthlyDatas.Find(id);
            if (axMonthlyData == null)
            {
                return NotFound();
            }

            db.AxMonthlyDatas.Remove(axMonthlyData);
            db.SaveChanges();

            return Ok(axMonthlyData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AxMonthlyDataExists(DateTime id)
        {
            return db.AxMonthlyDatas.Count(e => e.CreatedDate == id) > 0;
        }
    }
}