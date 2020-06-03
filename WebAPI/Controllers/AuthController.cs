using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class AuthController : ApiController
    {
        private ParknGardenData db = new ParknGardenData();


        [ResponseType(typeof(string))]
        [Route("api/Auth/GetSalt/{username}")]
        public IHttpActionResult GetSalt(string username)
        {
            string salt = AuthHandler.GetSalt(username, db);
            if (salt == null)
                return NotFound();
            return Ok(salt);
        }

        // GET: api/Auth/Login/username/passwordhash
        [ResponseType(typeof((int UserID, string SessionKey)))]
        [Route("api/Auth/Login/{username}/{password}")]
        public IHttpActionResult GetAuth(string username, string password)
        {
            (int UserID, string SessionKey) sessionInformation = AuthHandler.Login(username, password, db);
            if (sessionInformation.SessionKey == null)
                return NotFound();
            return Ok(sessionInformation);
        }

        // GET: api/Auth/GetUserlevel/sessionkey
        [ResponseType(typeof(int))]
        [Route("api/Auth/GetUserLevel/{sessionKey}")]
        public IHttpActionResult GetUserLevel(string sessionKey)
        {
            int userLevel = AuthHandler.GetUserLevel(sessionKey, db);
            if (userLevel == -1)
                return NotFound();
            return Ok(userLevel);
        }

        // GET: api/Auth/GetUserName
        [ResponseType(typeof(string))]
        [Route("api/Auth/GetUserName/{userID}")]
        public IHttpActionResult GetUserName(int userID)
        {
            string userName = AuthHandler.GetUserName(userID, db);
            if (userName == null)
            {
                return NotFound();
            }

            return Ok(userName);
        }

        // GET: api/Auth/CheckUserName
        [ResponseType(typeof(bool))]
        [Route("api/Auth/CheckUserName/{userName}")]
        public IHttpActionResult GetUserNameExists(string userName)
        {
            if (AuthHandler.CheckUserName(userName, db))
                return Ok(true);
            return NotFound();
        }

        // DELETE: api/Auth/DeleteSession/sessionKey/loggedId/sessionKey
        [ResponseType(typeof(Session))]
        [Route("api/Auth/DeleteSession/{sessionKey}/{loggedId}/{sessionKey}")]
        public IHttpActionResult DeleteSession(string sessionKey)
        {
            Session session = AuthHandler.DeleteSession(sessionKey, db);
            if (session == null)
                return NotFound();
            return Ok(session);
        }

        // PUT: api/Auth/PutAuth/id/loggedId/sessionKey
        [ResponseType(typeof(void))]
        [Route("api/Auth/PutAuth/{id}/{loggedId}/{sessionKey}")]
        public IHttpActionResult PutAuth(int id, Auth auth, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != auth.UserID)
            {
                return BadRequest();
            }
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.Entry(auth).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthExists(id))
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

        // POST: api/Auths/PostAuth/loggedId/sessionKey
        [Route("api/Auth/PostAuth/{loggedId}/{sessionKey}")]
        [ResponseType(typeof(Auth))]
        public async Task<IHttpActionResult> PostAuth(Auth auth, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                AuthHandler.PostNewAuth(auth, db);
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (AuthExists(auth.UserID))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtRoute("DefaultApi", new {id = auth.UserID}, auth);
            }
            return StatusCode(CommonMethods.StatusCodeReturn(error));
        }

        // DELETE: api/Auths/DeleteUserAuth/id/loggedId/sessionKey
        [Route("api/Auths/DeleteUserAuth/{id}/{loggedId}/{sessionKey}")]
        [ResponseType(typeof(Auth))]
        public IHttpActionResult DeleteAuth(int id, int loggedId, string sessionKey)
        {
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            Auth auth = db.Auths.Find(id);
            if (error == Constants.VerifyUserErrors.OK)
            {
                if (auth == null)
                {
                    return NotFound();
                }

                if (auth.UserID != 0)
                {
                    AuthHandler.DeleteUserAuth(db, auth);
                    return Ok(auth);
                }

                auth.UserID = -1;
                return Ok(auth);
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

        private bool AuthExists(int id)
        {
            return db.Auths.Count(e => e.UserID == id) > 0;
        }
    }
}