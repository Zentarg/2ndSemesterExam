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
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class InvoicesController : ApiController
    {
        private ParknGardenData db = new ParknGardenData();

        // GET: api/Invoices
        [ResponseType(typeof(Dictionary<int, Invoice>))]
        public IHttpActionResult GetInvoices()
        {
            return Ok(InvoiceHandler.GetAllInvoices(db));
        }

        // GET: api/InvoicesFromStores
        [ResponseType(typeof(Dictionary<int, List<int>>))]
        [Route("api/InvoicesFromStores")]
        public IHttpActionResult GetInvoicesFromStore()
        {
            return Ok(InvoiceHandler.GetAllInvoiceIDsFromStores(db));
        }

        // GET: api/InvoicesHasItems
        [ResponseType(typeof(Dictionary<int, KeyValuePair<int, int>>))]
        [Route("api/InvoicesHasItems")]
        public IHttpActionResult GetInvoicesHasItems()
        {
            return Ok(InvoiceHandler.GetAllInvoicesHasItems(db));
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

        // PUT: api/Invoices/5/loggedId/sessionKey
        [Route("api/Invoices/{id}/{loggedId}/{sessionKey}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutInvoice(int id, Invoice invoice, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != invoice.ID)
            {
                return BadRequest();
            }
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.Entry(invoice).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                    User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                    if (loggedUser != null)
                        LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has updated the invoice with an ID of {invoice.ID}", (int)LogHandler.RequestTypes.PUT);
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
            return StatusCode(CommonMethods.StatusCodeReturn(error));
        }

        // POST: api/Invoices/loggedId/sessionKey
        [ResponseType(typeof(Invoice))]
        [Route("api/Invoices/{loggedId}/{sessionKey}")]
        public IHttpActionResult PostInvoice(Invoice invoice, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /*Invoice newInvoice = db.Invoices.Add(invoice.Item1);
            db.SaveChanges();
            foreach (InvoiceHasItem item in invoice.Item2)
            {
                item.InvoiceID = newInvoice.ID;
                db.InvoiceHasItems.Add(item);
            }
            db.SaveChanges();


            return CreatedAtRoute("DefaultApi", new { id = invoice.Item1.ID }, invoice.Item1);*/
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {

                Invoice newInvoice = db.Invoices.Add(invoice);
                db.SaveChanges();
                User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                if (loggedUser != null)
                    LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has created the invoice with an ID of {invoice.ID}", (int)LogHandler.RequestTypes.POST);
                return CreatedAtRoute("DefaultApi", new {id = newInvoice.ID}, newInvoice);
            }
            return StatusCode(CommonMethods.StatusCodeReturn(error));

        }

        // DELETE: api/Invoices/DeleteInvoiceByUser/id/loggedId/sessionKey
        [Route ("api/Invoices/DeleteInvoiceByUser/{id}/{loggedId}/{sessionKey}")]
        [ResponseType(typeof(Invoice))]
        public IHttpActionResult DeleteInvoice(int id, int loggedId, string sessionKey)
        {
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            Invoice invoice = db.Invoices.Find(id);
            if (error == Constants.VerifyUserErrors.OK)
            {
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
                        User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                        if (loggedUser != null)
                            LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has deleted the invoice with an ID of {invoice.ID}", (int)LogHandler.RequestTypes.DELETE);
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

        private bool InvoiceExists(int id)
        {
            return db.Invoices.Count(e => e.ID == id) > 0;
        }
    }
}