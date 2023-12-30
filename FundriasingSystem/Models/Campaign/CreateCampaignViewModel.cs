using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FundriasingSystem.Models.Campaign
{
    public class CreateCampaignViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int TargetAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public Guid StaffId { get; set; }
        public Guid CampaignTypeId { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
        public List<SelectListItem> CampaignTypes { get; set; }
    }
}
