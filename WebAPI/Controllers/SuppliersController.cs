using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class SuppliersController : ApiController
    {
        private ParknGardenData db = new ParknGardenData();

        // GET: api/Suppliers
        [ResponseType(typeof(Dictionary<int,Supplier>))]
        public IHttpActionResult GetSuppliers()
        {
            Dictionary<int, Supplier> Suppliers = SupplierHandler.GetAllSupplier(db);

            if (Suppliers.Count==0)
            {
                return NotFound();
            }

            return Ok(Suppliers);
        }

        // GET: api/Suppliers/5
        [ResponseType(typeof(Supplier))]
        public async Task<IHttpActionResult> GetSupplier(int id)
        {
            Supplier supplier = await db.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            return Ok(supplier);
        }

        // PUT: api/Suppliers/UpdateSupplier/id
        [ResponseType(typeof(void))]
        [Route("api/Suppliers/UpdateSupplier/{id}")]
        public async Task<IHttpActionResult> PutSupplier(int id, Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != supplier.ID)
            {
                return BadRequest();
            }

            db.Entry(supplier).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
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

        // POST: api/Suppliers
        [ResponseType(typeof(Supplier))]
        public async Task<IHttpActionResult> PostSupplier(Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Suppliers.Add(supplier);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = supplier.ID }, supplier);
        }

        // DELETE: api/Suppliers/5
        [ResponseType(typeof(Supplier))]
        public async Task<IHttpActionResult> DeleteSupplier(int id)
        {
            Supplier supplier = await db.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            db.Suppliers.Remove(supplier);
            await db.SaveChangesAsync();

            return Ok(supplier);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SupplierExists(int id)
        {
            return db.Suppliers.Count(e => e.ID == id) > 0;
        }
    }
}