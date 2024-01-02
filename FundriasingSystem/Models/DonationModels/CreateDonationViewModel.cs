using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FundriasingSystem.Models.DonationModels
{
    public class CreateDonationViewModel
    {
        [Required]
        public int DonationAmount { get; set; }
        public IFormFile PaymentVoucherFile { get; set; }
        public string Remark { get; set; }
        [Required]
        public Guid CampaignId { get; set; }
        [Required]
        public Guid PaymentMethodId { get; set; }
        public List<SelectListItem> Campaigns { get; set; }
        public List<SelectListItem> PaymentMethods { get; set; }
    }
}
