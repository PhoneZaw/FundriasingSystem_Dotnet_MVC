using FundraisingApp.Entities;
using System;

namespace FundriasingSystem.Entities
{
    public class Suggestion : BaseEntity
    {
        public string SuggestionType { get; set; }
        public string Message { get; set; }
        public Guid DonorId { get; set; }
        public Guid CampaignId { get; set; }
    }
}
