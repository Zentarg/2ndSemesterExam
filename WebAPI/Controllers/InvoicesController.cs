using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI;

namespace WebAPI.Controllers
{
    public class InvoicesController : ApiController
    {
        private ParknGardenData db = new ParknGardenData();

        // GET: api/Invoices
        public IQueryable<Invoice> GetInvoices()
        {
            return db.Invoices;
        }

        // GET: api/Invoices/5
        [ResponseType(typeof(Invoice))]
        public IHttpActionResult GetInvoice(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return NotFound();
            }

            return Ok(invoice);
        }

        // PUT: api/Invoices/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutInvoice(int id, Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != invoice.ID)
            {
                return BadRequest();
            }

            db.Entry(invoice).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
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

        // POST: api/Invoices
        [ResponseType(typeof(Invoice))]
        public IHttpActionResult PostInvoice(Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Invoices.Add(invoice);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = invoice.ID }, invoice);
        }

        // DELETE: api/Invoices/DeleteInvoiceByUser/id
        [Route ("api/Invoices/DeleteInvoiceByUser/{id}")]
        [ResponseType(typeof(Invoice))]
        public IHttpActionResult DeleteInvoice(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            if (invoice != null)
            {
                invoice.AuthorID = 0;
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != invoice.ID)
                    return BadRequest();

                db.Entry(invoice).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(id))
                        return NotFound();
                    throw;

                }
                return Ok(invoice);
            }

            return NotFound();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InvoiceExists(int id)
        {
            return db.Invoices.Count(e => e.ID == id) > 0;
        }
    }
}