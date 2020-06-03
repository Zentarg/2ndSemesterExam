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
using WebAPI;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class InvoiceHasItemsController : ApiController
    {
        private ParknGardenData db = new ParknGardenData();

        // GET: api/InvoiceHasItems
        public IQueryable<InvoiceHasItem> GetInvoiceHasItems()
        {
            return db.InvoiceHasItems;
        }

        // GET: api/InvoiceHasItems/5
        [ResponseType(typeof(InvoiceHasItem))]
        public IHttpActionResult GetInvoiceHasItem(int id)
        {
            InvoiceHasItem invoiceHasItem = db.InvoiceHasItems.Find(id);
            if (invoiceHasItem == null)
            {
                return NotFound();
            }

            return Ok(invoiceHasItem);
        }

        // PUT: api/InvoiceHasItems/5/loggedId/sessionKey
        [Route("api/InvoiceHasItems/{id}/{loggedId}/{sessionKey}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutInvoiceHasItem(int id, InvoiceHasItem invoiceHasItem, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != invoiceHasItem.InvoiceID)
            {
                return BadRequest();
            }
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.Entry(invoiceHasItem).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceHasItemExists(id))
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
            return StatusCode(CommonMethods.StatusCodeReturn(error));
        }

        // POST: api/InvoiceHasItems/loggedId/sessionKey
        [Route("api/InvoiceHasItems/{loggedId}/{sessionKey}")]
        [ResponseType(typeof(InvoiceHasItem))]
        public IHttpActionResult PostInvoiceHasItem(InvoiceHasItem invoiceHasItem, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.InvoiceHasItems.Add(invoiceHasItem);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (InvoiceHasItemExists(invoiceHasItem.InvoiceID))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtRoute("DefaultApi", new {id = invoiceHasItem.InvoiceID}, invoiceHasItem);
            }
            return StatusCode(CommonMethods.StatusCodeReturn(error));
        }

        // DELETE: api/InvoiceHasItems/5/loggedId/sessionKey
        [ResponseType(typeof(InvoiceHasItem))]
        [Route("api/InvoiceHasItems/{id}/{loggedId}/{sessionKey}")]
        public IHttpActionResult DeleteInvoiceHasItem(int id, int loggedId, string sessionKey)
        {
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            InvoiceHasItem invoiceHasItem = db.InvoiceHasItems.Find(id);
            if (error == Constants.VerifyUserErrors.OK)
            {
                if (invoiceHasItem == null)
                {
                    return NotFound();
                }

                db.InvoiceHasItems.Remove(invoiceHasItem);
                db.SaveChanges();

                return Ok(invoiceHasItem);
            }
            return StatusCode(CommonMethods.StatusCodeReturn(error));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InvoiceHasItemExists(int id)
        {
            return db.InvoiceHasItems.Count(e => e.InvoiceID == id) > 0;
        }
    }
}