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
    public class RolesController : ApiController
    {
        private ParknGardenData db = new ParknGardenData();

        // GET: api/Roles
        [ResponseType(typeof(Dictionary<int, Role>))]
        public IHttpActionResult GetRoles()
        {
            Dictionary<int, Role> roles = RolesHandler.GetAllRoles(db);
            if (roles.Count == 0)
            {
                return NotFound();
            }
            return Ok(roles);
        }

        // GET: api/Roles/5
        [ResponseType(typeof(Role))]
        public IHttpActionResult GetRole(int id)
        {
            Role role = RolesHandler.GetOneRole(id, db);
            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        // PUT: api/Roles/5/loggedId/sessionKey
        [ResponseType(typeof(void))]
        [Route("api/Roles/{id}/{loggedId}/{sessionKey}")]
        public IHttpActionResult PutRole(int id, Role role, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != role.ID)
            {
                return BadRequest();
            }
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.Entry(role).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                    User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                    if (loggedUser != null)
                        LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has updated the role {role.Name} (ID: {role.ID})", (int)LogHandler.RequestTypes.PUT);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(id))
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

        // POST: api/Roles/loggedId/sessionKey
        [Route("api/Roles/{loggedId}/{sessionKey}")]
        [ResponseType(typeof(Role))]
        public IHttpActionResult PostRole(Role role, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                Role postedRole = RolesHandler.PostRole(db, role);
                User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                if (loggedUser != null)
                    LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has created the role {role.Name} (ID: {role.ID})", (int)LogHandler.RequestTypes.POST);
                return CreatedAtRoute("DefaultApi", new { id = postedRole.ID }, postedRole);
            }
            return StatusCode(CommonMethods.StatusCodeReturn(error));
        }

        // DELETE: api/Roles/5/loggedId/sessionKey
        [Route("api/Roles/{id}/{loggedId}/{sessionKey}")]
        [ResponseType(typeof(Role))]
        public IHttpActionResult DeleteRole(int id, int loggedId, string sessionKey)
        {
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            Role role = db.Roles.Find(id);
            if (error == Constants.VerifyUserErrors.OK)
            {
                if (role == null)
                {
                    return NotFound();
                }

                db.Roles.Remove(role);
                db.SaveChanges();
                User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                if (loggedUser != null)
                    LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has deleted the role {role.Name} (ID: {role.ID})", (int)LogHandler.RequestTypes.DELETE);
                return Ok(role);
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

        private bool RoleExists(int id)
        {
            return db.Roles.Count(e => e.ID == id) > 0;
        }
    }
}