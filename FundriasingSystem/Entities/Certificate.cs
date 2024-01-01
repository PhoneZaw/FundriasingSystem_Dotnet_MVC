using FundraisingApp.Entities;
using System;

namespace FundriasingSystem.Entities
{
    public class Certificate : BaseEntity
    {
        public Guid DonorId { get; set; }
        public Guid CampaignId { get; set; }
    }
}
