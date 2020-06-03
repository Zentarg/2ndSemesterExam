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
    public class StoresController : ApiController
    {
        private ParknGardenData db = new ParknGardenData();

        // GET: api/Stores
        [ResponseType(typeof(Dictionary<int, Dictionary<int, int>>))]
        public IHttpActionResult GetStocks()
        {
            Dictionary<int, Store> stores = StoresHandler.GetAllStores(db);

            if (stores.Count == 0)
                return NotFound();
            return Ok(stores);
        }

        // GET: api/Stores/5
        [ResponseType(typeof(Store))]
        public async Task<IHttpActionResult> GetStore(int id)
        {
            Store store = await db.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }

            return Ok(store);
        }

        // PUT: api/Stores/PutStore/id/loggedId/sessionKey
        [Route("api/Stores/PutStore/{id}/{loggedId}/{sessionKey}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStore(int id, Store store, string sessionKey, int loggedId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != store.ID)
            {
                return BadRequest();
            }

            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.Entry(store).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreExists(id))
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

        // POST: api/Stores/loggedId/sessionKey
        [ResponseType(typeof(Store))]
        [Route("api/Stores/{loggedId}/{sessionKey}")]
        public async Task<IHttpActionResult> PostStore(Store store, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.Stores.Add(store);
                await db.SaveChangesAsync();

                return CreatedAtRoute("DefaultApi", new { id = store.ID }, store);
            }

            return StatusCode(CommonMethods.StatusCodeReturn(error));
        }

        // DELETE: api/Stores/DeleteStore/5/loggedId/sessionKey
        [Route("api/Stores/DeleteStore/{id}/{loggedId}/{sessionKey}")]
        [ResponseType(typeof(Store))]
        public async Task<IHttpActionResult> DeleteStore(int id, int loggedId, string sessionKey)
        {
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            Store store = await db.Stores.FindAsync(id);
            if (error == Constants.VerifyUserErrors.OK)
            {
                if (store == null)
                {
                    return NotFound();
                }

                StoresHandler.DeleteOneStore(db, store);
                await db.SaveChangesAsync();

                return Ok(store);
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

        private bool StoreExists(int id)
        {
            return db.Stores.Count(e => e.ID == id) > 0;
        }
    }
}