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
using WebAPI2.Models;

namespace WebAPI2.Controllers
{
    [RoutePrefix("clients")]
    public class ClientsController : ApiController
    {
        private Fabrics2012Entities db = new Fabrics2012Entities();

        public ClientsController() {
            db.Configuration.LazyLoadingEnabled = false;
        }
        // GET: api/Clients
        [Route("")]
        public IQueryable<Client> GetClient()
        {
            return db.Client.Take(5);
        }

        // GET: api/Clients/5
        [ResponseType(typeof(Client))]
        [Route("{id}", Name = nameof(GetClientById))]
        public IHttpActionResult GetClientById(int id)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [ResponseType(typeof(Client))]
        [Route("{id}/order")]
        public IHttpActionResult GetClientOrders(int id)
        {
            var orders = db.Order.Where(p => p.ClientId == id).ToList();

            return Ok(orders);
        }

        [ResponseType(typeof(Client))]
        [Route("{id}/order/{date:datetime}")]
        public IHttpActionResult GetClientOrdersByDate1(int id, DateTime date)
        {
            var nextDay = date.AddDays(1);
            var orders = db.Order.Where(p => p.ClientId == id && p.OrderDate >= date && p.OrderDate < nextDay).ToList();

            return Ok(orders);
        }

        [ResponseType(typeof(Client))]
        [Route("{id}/order/{*date:datetime}")]
        public IHttpActionResult GetClientOrdersByDate2(int id, DateTime date)
        {
            var nextDay = date.AddDays(1);
            var orders = db.Order.Where(p => p.ClientId == id && p.OrderDate >= date && p.OrderDate < nextDay).ToList();

            return Ok(orders);
        }

        // PUT: api/Clients/5
        [ResponseType(typeof(void))]
        [Route("{id}")]
        public IHttpActionResult PutClient(int id, Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != client.ClientId)
            {
                return BadRequest();
            }

            db.Entry(client).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        [ResponseType(typeof(Client))]
        [Route("")]
        public IHttpActionResult PostClient(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Client.Add(client);
            db.SaveChanges();

            return CreatedAtRoute(nameof(GetClientById), new { id = client.ClientId }, client);
        }

        // DELETE: api/Clients/5
        [ResponseType(typeof(Client))]
        [Route("{id}")]
        public IHttpActionResult DeleteClient(int id)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            db.Client.Remove(client);
            db.SaveChanges();

            return Ok(client);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientExists(int id)
        {
            return db.Client.Count(e => e.ClientId == id) > 0;
        }
    }
}