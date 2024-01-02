using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FundriasingSystem.Models.CampaignModels
{
    public class CreateCampaignViewModel
    {
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
        public List<IFormFile> ImageFiles { get; set; }
        public List<SelectListItem> CampaignTypes { get; set; }
    }
}
