using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class StocksController : ApiController
    {
        private ParknGardenData db = new ParknGardenData();

        // GET: api/Stocks
        [ResponseType(typeof(Dictionary<int, Dictionary<int, int>>))]
        public IHttpActionResult GetStocks()
        {
            Dictionary<int, Stock> stocks = StocksHandler.GetAllStocks(db);

            if (stocks.Count == 0)
                return NotFound();
            return Ok(stocks);
        }

        // GET: api/Stocks/5
        [ResponseType(typeof(Stock))]
        public async Task<IHttpActionResult> GetStock(int id)
        {
            Stock stock = await db.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock);
        }

        // PUT: api/Stocks/5/loggedId/sessionKey
        [ResponseType(typeof(void))]
        [Route("api/Stocks/{id}/{loggedId}/{sessionKey}")]
        public async Task<IHttpActionResult> PutStock(int id, Stock stock, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stock.ID)
            {
                return BadRequest();
            }

            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.Entry(stock).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(id))
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

        // POST: api/Stocks/loggedId/sessionKey
        [ResponseType(typeof(Stock))]
        [Route("api/Stocks/{loggedId}/{sessionKey}")]
        public async Task<IHttpActionResult> PostStock(Stock stock, int loggedId, string sessionKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            if (error == Constants.VerifyUserErrors.OK)
            {
                db.Stocks.Add(stock);
                await db.SaveChangesAsync();

                return CreatedAtRoute("DefaultApi", new { id = stock.ID }, stock);
            }

            return StatusCode(CommonMethods.StatusCodeReturn(error));
        }

        // DELETE: api/Stocks/5
        [ResponseType(typeof(Stock))]
        [Route("api/Stocks/{id}/{loggedId/{sessionKey}")]
        public async Task<IHttpActionResult> DeleteStock(int id, int loggedId, string sessionKey)
        {

            Constants.VerifyUserErrors error = AuthHandler.VerifyUserSession(sessionKey, loggedId, db);
            Stock stock = await db.Stocks.FindAsync(id);
            if (error == Constants.VerifyUserErrors.OK)
            {
                if (stock == null)
                {
                    return NotFound();
                }

                db.Stocks.Remove(stock);
                await db.SaveChangesAsync();

                return Ok(stock);
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

        private bool StockExists(int id)
        {
            return db.Stocks.Count(e => e.ID == id) > 0;
        }
    }
}