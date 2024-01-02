using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace FundriasingSystem.Models.CampaignModels
{
    public class EditCampaignViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int TargetAmount { get; set; }
        [Required]
        public DateTime TargetDate { get; set; }
        [Required]
        public Guid StaffId { get; set; }
        [Required]
        public Guid CampaignTypeId { get; set; }
        [Required]
        public string Images { get; set; }
        public List<SelectListItem> CampaignTypes { get; set; }
    }
}
