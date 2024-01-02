using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FundriasingSystem.Models.PaymentMethod
{
    public class CreatePaymentMethodViewModel
    {
        [Required]
        public string PaymentMethodName { get; set; }
        [Required]
        public IFormFile IconFile { get; set; }
        [Required]
        public bool IsVerificationRequired { get; set; }
    }
}
