using System.ComponentModel.DataAnnotations;

namespace FundriasingSystem.Models.CampaignType
{
    public class CreateCampaignTypeViewModel
    {
        [Required]
        public string CampaignTypeName { get; set; }
    }
}
