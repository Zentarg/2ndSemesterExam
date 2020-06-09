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
    public class ItemsController : ApiController
    {
        private ParknGardenData db = new ParknGardenData();

        // GET: api/Items
        [ResponseType(typeof(Dictionary<int, Dictionary<int, int>>))]
        public IHttpActionResult GetItems()
        {
            Dictionary<int, Item> items = ItemsHandler.GetAllItems(db);

            if (items.Count == 0)
                return NotFound();
            return Ok(items);
        }

        // GET: api/Items/5
        [ResponseType(typeof(Item))]
        public IHttpActionResult GetItem(int id)
        {
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // PUT: api/Items/5/loggedId/sessionKey
        [ResponseType(typeof(void))]
        [Route("api/Items/{id}/{loggedId}/{sessionKey}")]
        public IHttpActionResult PutItem(int id, Item item, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.ID)
            {
                return BadRequest();
            }
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.Entry(item).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                    User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                    if (loggedUser != null)
                        LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has updated the item {item.Name} (ID: {item.ID})", (int)LogHandler.RequestTypes.PUT);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(id))
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

        // POST: api/Items/loggedId/sessionKey
        [ResponseType(typeof(Item))]
        [Route("api/Items/{loggedId}/{sessionKey}")]
        public IHttpActionResult PostItem(Item item, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.Items.Add(item);
                db.SaveChanges();
                User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                if (loggedUser != null)
                    LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has created the item {item.Name} (ID: {item.ID})", (int)LogHandler.RequestTypes.POST);
                return CreatedAtRoute("DefaultApi", new { id = item.ID }, item);
            }
            return StatusCode(CommonMethods.StatusCodeReturn(error));
        }

        // DELETE: api/Items/5/loggedId/sessionKey
        [ResponseType(typeof(Item))]
        [Route("api/Items/{id}/{loggedId}/{sessionKey}")]
        public IHttpActionResult DeleteItem(int id, int loggedId, string sessionKey)
        {
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            Item item = db.Items.Find(id);
            if (error == Constants.VerifyUserErrors.OK)
            {
                if (item == null)
                {
                    return NotFound();
                }

                db.Items.Remove(item);
                db.SaveChanges();
                User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                if (loggedUser != null)
                    LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has deleted the item {item.Name} (ID: {item.ID})", (int)LogHandler.RequestTypes.DELETE);
                return Ok(item);
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

        private bool ItemExists(int id)
        {
            return db.Items.Count(e => e.ID == id) > 0;
        }
    }
}