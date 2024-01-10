using FundraisingApp.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace FundriasingSystem.Models.SuggestionModels
{
    public class EditSuggestionViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public string SuggestionType { get; set; }
        [Required]
        public string Message { get; set; }
        public Guid DonorId { get; set; }
        public Guid CampaignId { get; set; }
        public Donor Donor { get; set; }
        public Campaign Campaign { get; set; }
    }
}
