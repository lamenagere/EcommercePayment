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
            payment.isPaid = false;
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
            if (ModelState.IsValid && PaymentExists(payment.Id))
            {
                var paymentPromess = _context.Payments.Single(p => p.Id == payment.Id);
                if (paymentPromess.paymentAmount == 0)
                {
                    return Forbid("The amount of this payment was not registered correctly in the promess... Check the payment promess you previously post to ~/api/payments");
                }
                if (paymentPromess.paymentAmount > 10000)
                {
                    try
                    {
                        paymentPromess.cardholderName = payment.cardholderName;
                        paymentPromess.cardNumber = payment.cardNumber;
                        paymentPromess.cvv = payment.cvv;
                        paymentPromess.expiryDate = payment.expiryDate;
                        paymentPromess.isPaid = true;
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        var result = StatusCode(StatusCodes.Status500InternalServerError, ex);
                        return result;
                    }
                    return CreatedAtAction("GetPayment", new { id = payment.Id }, payment.Id);
                }
                else
                {
                    return Unauthorized();
                }
            }

            return NotFound();
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

        [HttpGet("check/{id:int}")]
        public async Task<ActionResult> CheckPayment(int id)
        {
            if (PaymentExists(id))
            {
                var paymentToCheck = await _context.Payments.FindAsync(id);

                return Ok(paymentToCheck.isPaid);
            }

            return NotFound();
        }
    }
}
