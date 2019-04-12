using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EcommercePayment.Models
{
    public class PaymentModel
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string cardNumber { get; set; }
        [Required]
        [MinLength(4)]
        public string cardholderName { get; set; }
        [Required]
        [MinLength(3)]
        public string cvv { get; set; }
        [Required]
        public DateTime expiryDate { get; set; }
        [Required]
        public decimal paymentAmount { get; set; }
    }
}
