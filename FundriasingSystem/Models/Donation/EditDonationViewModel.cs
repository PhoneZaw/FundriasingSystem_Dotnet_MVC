using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace FundriasingSystem.Models.Donation
{
    public class EditDonationViewModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int DonationAmount { get; set; }
        public string PaymentVoucherUrl { get; set; }
        public string Remark { get; set; }
        public bool IsVerified { get; set; } = false;
        public Guid DonorId { get; set; }
        public Guid CampaignId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public List<SelectListItem> Campaigns { get; set; }
        public List<SelectListItem> PaymentMethods { get; set; }
    }
}
