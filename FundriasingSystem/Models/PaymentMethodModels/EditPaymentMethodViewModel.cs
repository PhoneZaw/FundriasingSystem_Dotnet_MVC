using System;
using System.ComponentModel.DataAnnotations;

namespace FundriasingSystem.Models.PaymentMethodModels
{
    public class EditPaymentMethodViewModel
    {
        //[Required]
        public Guid Id { get; set; }
        //[Required]
        public string Status { get; set; }
        //[Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public string PaymentMethodName { get; set; }
        [Required]
        public string Description { get; set; }
        //[Required]
        public string PaymentMethodIconUrl { get; set; }
        //[Required]
        public bool IsVerificationRequired { get; set; }
    }
}
