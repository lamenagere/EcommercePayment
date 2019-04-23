using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommercePaymentData;
using EcommercePaymentData.Entities;
using EcommercePaymentServices;
using EcommerceCommon;

namespace EcommercePaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentContext _context;
        private readonly IPaymentService service;

        public PaymentsController(PaymentContext context, IPaymentService service)
        {
            _context = context;
            this.service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        // GET: api/Payments/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = new Payment();

            try
            {
                payment = await _context.Payments.FindAsync(id);
            }
            catch (Exception)
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
        /// <returns></returns>
        // GET: api/Payments/1b-0E-.....
        [HttpGet("{paymentId}")]
        public async Task<ActionResult<Payment>> GetPayment(string paymentId)
        {
            var payment = new Payment();

            try
            {
                payment = await _context.Payments.FirstOrDefaultAsync(p => p.guid == paymentId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if (payment == null || payment.id == 0)
            {
                return NotFound();
            }

            return payment;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="payment"></param>
        /// <returns></returns>
        // PUT: api/Payments/5
        [HttpPut("{paymentId}")]
        public async Task<IActionResult> PutPayment(string paymentId, Payment payment)
        {
            if (paymentId != payment.guid)
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
                throw;
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
        public async Task<ActionResult<Payment>> PostPayment(PaymentPromessModel paymentPromess)
        {
            var payment = new Payment();

            if (paymentPromess.amount == 0)
                return ValidationProblem();
            else
            {
                payment.paymentAmount = (float)paymentPromess.amount;
                // TODO: changer le moddèle.guiod de String à Guid
                payment.guid = service.CreateGuid();
            }

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            Response.Headers.Add(service.BuildGuidHeaders(payment.guid));
            return CreatedAtAction("GetPayment", new { paymentId = payment.guid }, payment.guid);
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
                if (payment.id == 0)
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
            if (ModelState.IsValid && PaymentExists(payment.id))
            {
                var paymentPromess = _context.Payments.Single(p => p.id == payment.id);
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
                    return CreatedAtAction("GetPayment", new { paymentId = payment.id }, payment.id);
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
            return _context.Payments.Any(e => e.id == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool PaymentExists(string guid)
        {
            return _context.Payments.Any(e => e.guid == guid);
        }

        [HttpGet]
        [Route("check/{paymentId}")]
        public async Task<ActionResult> CheckPayment(string paymentId)
        {
            if (PaymentExists(paymentId))
            {
                var paymentToCheck = await _context.Payments.FirstOrDefaultAsync(p => p.guid == paymentId);

                return Ok(paymentToCheck.isPaid);
            }

            return NotFound();
        }
    }
}
