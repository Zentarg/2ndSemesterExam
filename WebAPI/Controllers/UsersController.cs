﻿using System;
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
    public class UsersController : ApiController
    {
        private ParknGardenData db = new ParknGardenData();

        // GET: api/Users
        [ResponseType((typeof(Dictionary<int, User>)))]
        public IHttpActionResult GetUsers()
        {
            Dictionary<int, User> users = UserHandler.GetAllUsers(db);
            if (users.Count == 0)
            {
                return NotFound();
            }

            return Ok(users);
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = UserHandler.GetOneUser(id, db);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // GET: api/Users/FilterById/id
        [ResponseType(typeof(Dictionary<int, User>))]
        [Route("api/Users/FilterByUserLevelId/{id}")]
        public IHttpActionResult GetUsersByLevelId(int id)
        {
            Dictionary<int, User> users = UserHandler.GetUsersByUserLevel(db, id);
            if (users.Count == 0)
            {
                return NotFound();
            }

            return Ok(users);
        }

        // PUT: api/Users/UpdateUser/id/loggedId/sessionKey
        [ResponseType(typeof(void))]
        [Route("api/Users/UpdateUser/{id}/{loggedId}/{sessionKey}")]
        public async Task<IHttpActionResult> PutUser(int id, User user, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.ID)
            {
                return BadRequest();
            }

            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.Entry(user).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                    User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                    if (loggedUser != null)
                        LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has updated the user {user.Name} (ID: {user.ID})", (int) LogHandler.RequestTypes.PUT);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
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

        // POST: api/Users/loggedId/sessionKey
        [Route("api/Users/{loggedId}/{sessionKey}")]
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                User postedUser = UserHandler.PostUser(db, user);
                User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                if (loggedUser != null)
                    LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has created the user {user.Name} (ID: {user.ID})", (int)LogHandler.RequestTypes.POST);
                return CreatedAtRoute("DefaultApi", new { id = postedUser.ID }, postedUser);
            }

            return StatusCode(CommonMethods.StatusCodeReturn(error));

        }

        // DELETE: api/Users/DeleteUser/id/loggedId/sessionKey
        [Route("api/Users/DeleteUser/{id}/{loggedId}/{sessionKey}")]
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id, int loggedId, string sessionKey)
        {
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            User user = db.Users.FirstOrDefault(u => u.ID == id);
            User returnUser = user;
            if (error == Constants.VerifyUserErrors.OK)
            {
                if (user == null)
                {
                    return NotFound();
                }

                if (user.ID != 0)
                {
                    UserHandler.DeleteOneUser(db, user);
                    returnUser.StoreID = 0;
                    User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                    if (loggedUser != null)
                    {
                        LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has deleted the user {user.Name} (ID: {user.ID})", (int)LogHandler.RequestTypes.DELETE);
                        LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has deleted the login information for {returnUser.Name} (ID: {user.ID})", (int)LogHandler.RequestTypes.DELETE);
                        LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has deleted the salary for {returnUser.Name} (ID: {user.ID})", (int)LogHandler.RequestTypes.DELETE);
                    }
                    
                    return Ok(returnUser);
                }

                user.ID = -1;
                return Ok(user);
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

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.ID == id) > 0;
        }
    }
}