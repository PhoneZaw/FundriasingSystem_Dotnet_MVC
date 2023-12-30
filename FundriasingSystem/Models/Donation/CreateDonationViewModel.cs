using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace FundriasingSystem.Models.Donation
{
    public class CreateDonationViewModel
    {
        public int DonationAmount { get; set; }
        public IFormFile PaymentVoucherFile { get; set; }  
        public string Remark { get; set; }
        public Guid DonorId { get; set; }
        public Guid CampaignId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public List<SelectListItem> Campaigns { get; set; }
        public List<SelectListItem> PaymentMethods { get; set; }
    }
}
