namespace FundriasingSystem.Models.PaymentMethod
{
    public class CreatePaymentMethodViewModel
    {
        public string PaymentMethodName { get; set; }
        public bool IsVerificationRequired { get; set; }
    }
}
