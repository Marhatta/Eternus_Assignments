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
using Authorization.Models;
using System.Collections.Specialized;
using System.Web;
using Microsoft.Owin;
using System.Security.Claims;

namespace Authorization.Controllers
{
    public class AuthController : ApiController
    {
        private ProductsEntities1 db = new ProductsEntities1();

        // GET api/Auth

        [HttpGet]
        public IQueryable<Product> GetAdmins()
        {
            return db.Products;
        }
        
        // GET api/Auth/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetAdmin(int id)
        {
           
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT api/Auth/5
        public IHttpActionResult PutAdmin(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST api/Auth
        [Authorize(Roles="admin")] 
        public List<Product> PostProducts(Product product)
        {
            var identity = User.Identity as ClaimsIdentity;
            var accessId = identity.FindFirst("Id").Value;

            var e = new Product()
            {
               Id = product.Id,
                Name = product.Name,
                Manufacturer = product.Manufacturer,
                AdminId=Convert.ToInt32(accessId)
            };
            db.Products.Add(e);
            db.SaveChanges();
            return db.Products.ToList();
        }
            
            
         

        // DELETE api/Auth/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}