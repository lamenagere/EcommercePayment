﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommercePayment.Pages.Payment
{
    public class PaymentModel : PageModel
    {
        public void OnGet()
        {

        }

        public IActionResult onPost(string test)
        {
            return RedirectToPage("Contact");
        }
    }
}