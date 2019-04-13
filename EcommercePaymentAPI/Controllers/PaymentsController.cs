using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommercePaymentData;
using EcommercePaymentData.Entities;

namespace EcommercePaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentContext _context;

        public PaymentsController(PaymentContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = new Payment();

            try
            {
                payment = await _context.Payments.FindAsync(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="payment"></param>
        /// <returns></returns>
        // PUT: api/Payments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, Payment payment)
        {
            if (id != payment.Id)
            {
                return BadRequest();
            }

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        // POST: api/Payments
        [HttpPost]
        public async Task<ActionResult<Payment>> PostPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayment", new { id = payment.Id }, payment.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Payments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Payment>> DeletePayment(int id)
        {
            var payment = new Payment();

            if (PaymentExists(id))
            {
                payment = await _context.Payments.FindAsync(id);
                if (payment.Id == 0)
                {
                    return NotFound();
                }

                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
            

            return payment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("process")]
        public ActionResult ProcessPayment(Payment payment)
        {
            if (PaymentExists(payment.Id))
            {
                if (_context.Payments.Single(p => p.Id == payment.Id).paymentAmount > 10000)
                {
                    return CreatedAtAction("GetPayment", new { id = payment.Id }, payment.Id);
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }
    }
}
