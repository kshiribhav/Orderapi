using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Infrastructure;
using OrderApi.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrderController : ControllerBase
    {
        private OrderContext db;
        private IConfiguration configuration;

        public OrderController(OrderContext db, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.db = db;
        }
        //Authorize(Roles = "user")]
        [HttpGet("",Name ="GetOrders")]
        public async Task<ActionResult<List<OrderItem>>> Get()
        {
            try
            {
                var result = await this.db.Order.FindAsync<OrderItem>(FilterDefinition<OrderItem>.Empty);
                return result.ToList();
            }
            catch(Exception ex) { return null; }
        }

        //[Authorize(Roles = "admin")]
        [HttpPost("",Name ="AddOrder")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult PlaceOrder(OrderItem item)
        {
            TryValidateModel(item);
            if(ModelState.IsValid)
            {
                this.db.Order.InsertOne(item);
                return Created("", item);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //[Authorize(Roles = "admin")]
        [HttpDelete("{id}", Name = "CancelOrder")]
        public async Task<ActionResult<OrderItem>> CancelOrderAsync(string id)
        {
            try
            {
                var filter = Builders<OrderItem>.Filter.Eq("_id", id);
                var item = this.db.Order.Find<OrderItem>(filter).FirstOrDefault();
                if (item != null)
                {

                    await this.db.Order.DeleteOneAsync<OrderItem>(c=> c.Id==item.Id);
                    return Ok(item);
                }
                else
                {
                    return NotFound();
                }           }
            catch (Exception ex) { return null; } //implement error handling 
        }

        //[Authorize(Roles = "admin")]
        [HttpGet("{id}", Name ="GetOrderbyId")]
        public ActionResult Get(string id)
        {
            var item = this.db.Order.Find<OrderItem>(c => c.Id == id).FirstOrDefault();
            if (item != null)
                return Ok(item);
            else
                return NotFound();
        }
        //[HttpDelete("{id}", Name = "DeleteOrder")]
        //public ActionResult Delete(string id)
        //{
        //    var item = this.db.Order.Find<OrderItem>(c => c.Id == id).FirstOrDefault();
        //    if (item != null)
        //    {
                
        //        var filter = Builders<OrderItem>.Filter.Eq("_id", id);
        //        this.db.Order.DeleteOne(filter);
        //        return Ok(item);
        //    }
                
        //    else
        //        return NotFound();
        //}
    }
}