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
    public class SalariesController : ApiController
    {
        private ParknGardenData db = new ParknGardenData();

        // GET: api/Salaries
        [ResponseType((typeof(Dictionary<int, Salary>)))]
        public IHttpActionResult GetSalaries()
        {
            Dictionary<int, Salary> salaries = SalaryHandler.GetAllSalaries(db);
            if (salaries.Count == 0)
            {
                return NotFound();
            }
            return Ok(salaries);
        }

        // GET: api/Salaries/5
        [ResponseType(typeof(Salary))]
        public IHttpActionResult GetSalary(int id)
        {
            Salary salary = SalaryHandler.GetOneSalary(id, db);
            if (salary == null)
            {
                return NotFound();
            }

            return Ok(salary);
        }

        // PUT: api/Salaries/UpdateSalary/id/loggedId/sessionKey
        [ResponseType(typeof(void))]
        [Route("api/Salaries/UpdateSalary/{id}/{loggedId}/{sessionKey}")]
        public async Task<IHttpActionResult> PutSalary(int id, Salary salary, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != salary.UserID)
            {
                return BadRequest();
            }

            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.Entry(salary).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                    User updatedUserSalary = db.Users.FirstOrDefault(u => u.ID == salary.UserID);
                    if (updatedUserSalary != null)
                    {
                        User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                        if (loggedUser != null)
                            LogHandler.CreateLogEntry(db, loggedId,
                            $"The user {loggedUser.Name} (ID: {loggedId}) has updated the salary for {updatedUserSalary.Name} (ID: {updatedUserSalary.ID})",
                            (int) LogHandler.RequestTypes.PUT);
                    }
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryExists(id))
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

        // POST: api/Salaries/loggedId/sessionKey
        [ResponseType(typeof(Salary))]
        [Route("api/Salaries/{loggedId}/{sessionKey}")]
        public IHttpActionResult PostSalary(Salary salary, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.Salaries.Add(salary);

                try
                {
                    db.SaveChanges();
                    User postedUserSalary = db.Users.FirstOrDefault(u => u.ID == salary.UserID);
                    if (postedUserSalary != null)
                    {
                        User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                        if (loggedUser != null)
                            LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has created the salary for {postedUserSalary.Name} (ID: {postedUserSalary.ID})", (int)LogHandler.RequestTypes.POST);
                    }
                        
                }
                catch (DbUpdateException)
                {
                    if (SalaryExists(salary.UserID))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtRoute("DefaultApi", new { id = salary.UserID }, salary);
            }

            return StatusCode(CommonMethods.StatusCodeReturn(error));
        }

        // DELETE: api/Salaries/DeleteSalary/id/loggedId/sessionKey
        [Route("api/Salaries/DeleteSalary/{id}/{loggedId}/{sessionKey}")]
        [ResponseType(typeof(Salary))]
        public IHttpActionResult DeleteSalary(int id, int loggedId, string sessionKey)
        {
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            Salary salary = db.Salaries.Find(id);
            if (error == Constants.VerifyUserErrors.OK)
            {
                if (salary == null)
                {
                    return NotFound();
                }

                if (salary.UserID != 0)
                {
                    SalaryHandler.DeleteOneSalary(db, salary);
                    User deletedUserSalary = db.Users.FirstOrDefault(u => u.ID == salary.UserID);
                    if (deletedUserSalary != null)
                    {
                        User loggedUser = db.Users.FirstOrDefault(u => u.ID == loggedId);
                        if (loggedUser != null)
                            LogHandler.CreateLogEntry(db, loggedId, $"The user {loggedUser.Name} (ID: {loggedId}) has deleted the salary for {deletedUserSalary.Name} (ID: {deletedUserSalary.ID})", (int)LogHandler.RequestTypes.DELETE);
                    }
                        
                    return Ok(salary);
                }

                salary.UserID = -1;
                return Ok(salary);
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

        private bool SalaryExists(int id)
        {
            return db.Salaries.Count(e => e.UserID == id) > 0;
        }
    }
}