using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;

namespace FundriasingSystem.Models.CampaignModels
{
    public class EditCampaignViewModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TargetAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public Guid StaffId { get; set; }
        public Guid CampaignTypeId { get; set; }
        public string Images { get; set; }
        public List<SelectListItem> CampaignTypes { get; set; }
    }
}
