using System;

namespace FundriasingSystem.Models.CampaignType
{
    public class EditCampaignTypeViewModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CampaignTypeName { get; set; }
    }
}
