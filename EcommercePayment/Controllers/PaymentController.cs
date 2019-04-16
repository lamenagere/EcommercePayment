using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult> Index([FromQuery]int paymentId, [FromQuery]string returnUrl)
        {
            var payment = new PaymentModel();
            try
            {
                payment = await service.GetPaymentAsync(paymentId);
            }
            catch(Exception ex)
            {
                throw;
            }
            
            if (payment == null) return StatusCode(StatusCodes.Status404NotFound);

            return View(payment);
        }

        [HttpPost]
        public async Task<ActionResult> Index([FromBody]PaymentModel payment, [FromQuery]string returnUrl, [FromQuery]int paymentId)
        {

            var client = new HttpClient();

            var result = await service.ProcessPayment(payment);

            if (result == true)
            {
                return RedirectToAction("Validated", routeValues: new { returnUrl = returnUrl });
            }
            return RedirectToAction("Rejected");
        }

        // GET: Payment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Validated(string returnUrl)
        {

            //ViewBag.returnUrl = returnUrl;
            return View(model: returnUrl);
        }

        [HttpGet]
        public ActionResult Rejected()
        {
            return View();
        }
    }
}