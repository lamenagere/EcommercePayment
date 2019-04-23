using System;
using System.Collections.Generic;
using System.Text;

namespace EcommercePaymentData.Entities
{
    public class Payment
    {
        public int id { get; set; }
        public string guid { get; set; }
        public string cardNumber { get; set; }
        public string cardholderName { get; set; }
        public string cvv { get; set; }
        public DateTime expiryDate { get; set; }
        public float paymentAmount { get; set; }
        public bool isPaid { get; set; }
    }
}
