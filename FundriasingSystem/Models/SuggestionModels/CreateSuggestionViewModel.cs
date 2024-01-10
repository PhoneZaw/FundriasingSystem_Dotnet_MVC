using FundraisingApp.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace FundriasingSystem.Models.SuggestionModels
{
    public class CreateSuggestionViewModel
    {
        public string SuggestionType { get; set; }
        [Required]
        public string Message { get; set; }
        public Guid DonorId { get; set; }
        public Guid CampaignId { get; set; }
    }
}
