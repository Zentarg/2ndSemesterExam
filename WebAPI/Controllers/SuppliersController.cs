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
using System.Web.Http.Routing.Constraints;
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

        // PUT: api/Suppliers/UpdateSupplier/id/loggedId/sessionKey
        [ResponseType(typeof(void))]
        [Route("api/Suppliers/UpdateSupplier/{id}/{loggedId}/{sessionKey}")]
        public async Task<IHttpActionResult> PutSupplier(int id, Supplier supplier, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != supplier.ID)
            {
                return BadRequest();
            }

            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.Entry(supplier).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                    User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                    if (loggedUser != null)
                        LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has updated the supplier {supplier.Name} (ID: {supplier.ID})", (int)LogHandler.RequestTypes.PUT);
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

            return StatusCode(CommonMethods.StatusCodeReturn(error));
        }

        // POST: api/Suppliers/loggedId/sessionKey
        [ResponseType(typeof(Supplier))]
        [Route("api/Suppliers/{loggedId}/{sessionKey}")]
        public async Task<IHttpActionResult> PostSupplier(Supplier supplier, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.Suppliers.Add(supplier);
                await db.SaveChangesAsync();
                User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                if (loggedUser != null)
                    LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has created the supplier {supplier.Name} (ID: {supplier.ID})", (int)LogHandler.RequestTypes.POST);

                return CreatedAtRoute("DefaultApi", new { id = supplier.ID }, supplier);
            }

            return StatusCode(CommonMethods.StatusCodeReturn(error));
        }

        // DELETE: api/Suppliers/DeleteSupplier/id/loggedId/sessionKey
        [Route("api/Suppliers/DeleteSupplier/{id}/{loggedId}/{sessionKey}")]
        [ResponseType(typeof(Supplier))]
        public async Task<IHttpActionResult> DeleteSupplier(int id, int loggedId, string sessionKey)
        {
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            Supplier supplier = await db.Suppliers.FindAsync(id);
            Supplier returnSupplier = supplier;
            if (error == Constants.VerifyUserErrors.OK)
            {
                if (supplier == null)
                {
                    return NotFound();
                }

                if (supplier.ID != 0)
                {
                    db.Suppliers.Remove(supplier);
                    await db.SaveChangesAsync();
                    User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                    if (loggedUser != null)
                        LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has deleted the supplier {supplier.Name} (ID: {supplier.ID})", (int)LogHandler.RequestTypes.DELETE);
                    return Ok(returnSupplier);
                }

                supplier.ID = -1;
                return Ok(supplier);
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

        private bool SupplierExists(int id)
        {
            return db.Suppliers.Count(e => e.ID == id) > 0;
        }
    }
}