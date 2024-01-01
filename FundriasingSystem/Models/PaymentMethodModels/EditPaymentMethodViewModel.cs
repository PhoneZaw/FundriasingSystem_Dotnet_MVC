using System;

namespace FundriasingSystem.Models.PaymentMethod
{
    public class EditPaymentMethodViewModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PaymentMethodName { get; set; }
        public string PaymentMethodIconUrl { get; set; }
        public bool IsVerificationRequired { get; set; }
    }
}
