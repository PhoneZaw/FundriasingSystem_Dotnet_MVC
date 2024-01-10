using FundraisingApp.Entities;
using System;

namespace FundriasingSystem.Models.SuggestionModels
{
    public class SuggestionViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SuggestionType { get; set; }
        public string Message { get; set; }
        public Guid DonorId { get; set; }
        public Guid CampaignId { get; set; }
        public Donor Donor { get; set; }
        public int TotalDonationAmount { get; set; }
        public string TimeDiff { get; set; }
    }
}
