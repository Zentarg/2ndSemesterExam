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
    public class CategoriesController : ApiController
    {
        private ParknGardenData db = new ParknGardenData();

        // GET: api/Categories
        [ResponseType(typeof(Dictionary<int,Category>))]
        public IHttpActionResult GetCategories()
        {
            Dictionary<int, Category> categories = CategoriesHandler.GetAllCategories(db);
            if (categories.Count==0)
            {
                return NotFound();
            }

            return Ok(categories);

        }

        // GET: api/Categories/5
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> GetCategory(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/Categories/5/loggedId/sessionKey
        [ResponseType(typeof(void))]
        [Route("api/Categories/{id}/{loggedId}/{sessionKey}")]
        public async Task<IHttpActionResult> PutCategory(int id, Category category, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.ID)
            {
                return BadRequest();
            }
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.Entry(category).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(id))
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

        // POST: api/Categories/loggedId/sessionKey
        [ResponseType(typeof(Category))]
        [Route("api/Categories/{loggedId}/{sessionKey}")]
        public async Task<IHttpActionResult> PostCategory(Category category, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();

                return CreatedAtRoute("DefaultApi", new {id = category.ID}, category);
            }
            return StatusCode(CommonMethods.StatusCodeReturn(error));
        }

        // DELETE: api/Categories/5/loggedId/sessionKey
        [ResponseType(typeof(Category))]
        [Route("api/Categories/{id}/{loggedId}/{sessionKey}")]
        public async Task<IHttpActionResult> DeleteCategory(int id, int loggedId, string sessionKey)
        {
            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            Category category = await db.Categories.FindAsync(id);
            if (error == Constants.VerifyUserErrors.OK)
            {
                if (category == null)
                {
                    return NotFound();
                }

                db.Categories.Remove(category);
                await db.SaveChangesAsync();

                return Ok(category);
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

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.ID == id) > 0;
        }
    }
}