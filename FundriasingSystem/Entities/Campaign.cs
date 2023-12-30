using System;

namespace FundraisingApp.Entities
{
    public class Campaign : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int TargetAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public Guid StaffId { get; set; }
        public Guid CampaignTypeId { get; set; }
        public string Images { get; set; }
        public Staff Staff { get; set; }
        public CampaignType CampaignType { get; set; }
    }
}
