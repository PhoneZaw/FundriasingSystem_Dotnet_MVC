using FundraisingApp.Entities;
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
        public int DonationAmount { get; set; } = 1000;
        public IFormFile PaymentVoucherFile { get; set; }
        public string Remark { get; set; }
        [Required]
        public Guid CampaignId { get; set; }
        [Required]
        public Guid PaymentMethodId { get; set; }
        public List<SelectListItem> Campaigns { get; set; }
        public IEnumerable<PaymentMethod> PaymentMethods { get; set; }
    }
}
