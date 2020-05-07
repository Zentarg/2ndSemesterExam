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
    public class UserLevelsController : ApiController
    {
        private ParknGardenData db = new ParknGardenData();

        // GET: api/UserLevels
        [ResponseType((typeof(Dictionary<int, User>)))]
        public IHttpActionResult GetUsers()
        {
            Dictionary<int, UserLevel> userLevels = UserLevelHandler.GetAllUserLevels(db);
            if (userLevels.Count == 0)
                return NotFound();
            return Ok(userLevels);
        }

        // GET: api/UserLevels/5
        [ResponseType(typeof(UserLevel))]
        public IHttpActionResult GetUserLevel(int id)
        {
            UserLevel userLevel = UserLevelHandler.GetOneUserLevel(id, db);
            if (userLevel == null)
                return NotFound();
            return Ok(userLevel);
        }

        // PUT: api/UserLevels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserLevel(int id, UserLevel userLevel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userLevel.ID)
            {
                return BadRequest();
            }

            db.Entry(userLevel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserLevelExists(id))
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

        // POST: api/UserLevels
        [ResponseType(typeof(UserLevel))]
        public IHttpActionResult PostUserLevel(UserLevel userLevel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserLevels.Add(userLevel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userLevel.ID }, userLevel);
        }

        // DELETE: api/UserLevels/5
        [ResponseType(typeof(UserLevel))]
        public IHttpActionResult DeleteUserLevel(int id)
        {
            UserLevel userLevel = db.UserLevels.Find(id);
            if (userLevel == null)
            {
                return NotFound();
            }

            db.UserLevels.Remove(userLevel);
            db.SaveChanges();

            return Ok(userLevel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserLevelExists(int id)
        {
            return db.UserLevels.Count(e => e.ID == id) > 0;
        }
    }
}