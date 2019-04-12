using EcommercePaymentData.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace EcommercePaymentData
{
    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> options)
            : base (options)
        {

        }
        public DbSet<Payment> Payments { get; set; }
    }
}
