namespace FundraisingApp.Entities
{
    public class PaymentMethod : BaseEntity
    {
        public string PaymentMethodName { get; set; }
        public string PaymentMethodIconUrl { get; set; }
        public bool IsVerificationRequired { get; set; }
    }
}
