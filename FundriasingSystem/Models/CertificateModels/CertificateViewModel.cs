using System;
using System.Collections.Generic;
using FundraisingApp.Entities;

namespace FundriasingSystem.Models.Certificate
{
    public class CertificateViewModel
    {
        public Guid Id { get; set; }
        public Guid DonorId { get; set; }
        public Donor Donor { get; set; }
        public Guid CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public List<Donation> Donations { get; set; }
        public int TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
