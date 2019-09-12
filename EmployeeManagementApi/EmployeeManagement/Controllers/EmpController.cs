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
using EmployeeManagement.Models;
using System.Web.Mvc;

namespace EmployeeManagement.Controllers
{
    public class EmpController : ApiController
    {
        private EmpDBEntities db = new EmpDBEntities();

        // GET api/Emp
        public IQueryable<emp> Getemp()
        {
            return db.emp;
        }

       


        // GET api/Emp/5
        [ResponseType(typeof(emp))]
        public IHttpActionResult Getemp(int id)
        {
            emp emp = db.emp.Find(id);
            if (emp == null)
            {
                return NotFound();
            }

            return Ok(emp);
        }

        // PUT api/Emp/5
        public IHttpActionResult Putemp(int id, emp emp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != emp.Id)
            {
                return BadRequest();
            }

            db.Entry(emp).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!empExists(id))
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

        // POST api/Emp
        [ResponseType(typeof(emp))]
        public IHttpActionResult Postemp(emp emp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.emp.Add(emp);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (empExists(emp.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = emp.Id }, emp);
        }

        // DELETE api/Emp/5
        [ResponseType(typeof(emp))]
        public IHttpActionResult Deleteemp(int id)
        {
            emp emp = db.emp.Find(id);
            if (emp == null)
            {
                return NotFound();
            }

            db.emp.Remove(emp);
            db.SaveChanges();

            return Ok(emp);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool empExists(int id)
        {
            return db.emp.Count(e => e.Id == id) > 0;
        }
    }
}