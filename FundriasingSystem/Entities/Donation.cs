using System;

namespace FundraisingApp.Entities
{
    public class Donation : BaseEntity
    {
        public int DonationAmount { get; set; }
        public string PaymentVoucherUrl { get; set; }
        public string Remark { get; set; }
        public bool IsVerified { get; set; } = false;
        public Guid DonorId { get; set; }
        public Guid CampaignId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public Donor Donor { get; set; }
        public Campaign Campaign { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
