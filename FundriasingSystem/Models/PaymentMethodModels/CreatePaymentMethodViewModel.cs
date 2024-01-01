using Microsoft.AspNetCore.Http;

namespace FundriasingSystem.Models.PaymentMethod
{
    public class CreatePaymentMethodViewModel
    {
        public string PaymentMethodName { get; set; }
        public IFormFile IconFile { get; set; }
        public bool IsVerificationRequired { get; set; }
    }
}
