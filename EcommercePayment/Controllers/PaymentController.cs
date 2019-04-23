using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EcommercePayment.Models;
using EcommercePayment.Services;
using EcommercePaymentData.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EcommercePayment.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private PaymentService service;

        public PaymentController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            service = new PaymentService();
        }
        // GET: Payment
        public async Task<ActionResult> Index([FromQuery]string paymentId, [FromQuery]string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(paymentId) || string.IsNullOrWhiteSpace(returnUrl)) return BadRequest();

            var payment = new PaymentModel();
            try
            {
                payment = await service.GetPaymentAsync(paymentId);
            }
            catch
            {
                throw;
            }

            if (payment.id == 0) return StatusCode(StatusCodes.Status404NotFound);

            return View(payment);
        }

        [HttpPost]
        public async Task<ActionResult> Index(PaymentModel payment, string returnUrl)
        {

            var client = new HttpClient();

            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var result = await service.ProcessPayment(payment);

            var completePayment = await service.GetPaymentAsync(payment.id);

            // TODO: modifier cette horreur
            returnUrl += "?paymentId=" + completePayment.guid;

            if (result == true)
            {
                return RedirectToAction("Validated", new { returnUrl = returnUrl});
            }
            return RedirectToAction("Rejected", new { returnUrl = returnUrl});
        }

        [HttpGet("validated/{returnUrl}")]
        public ActionResult Validated(string returnUrl)
        {

            ViewBag.returnUrl = WebUtility.UrlDecode(returnUrl);
            return View();
        }

        [HttpGet("rejected/{returnUrl}")]
        public ActionResult Rejected(string returnUrl)
        {
            ViewBag.returnUrl = WebUtility.UrlDecode(returnUrl);
            return View();
        }
    }
}