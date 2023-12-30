using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Enums;
using FundraisingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FundraisingApp.Services
{
    public class CampaignService
    {
        private readonly IRepository<Campaign> _CampaignRepository;
        private readonly IMapper _mapper;

        public CampaignService(IRepository<Campaign> CampaignRepository,
            IMapper mapper)
        {
            _CampaignRepository = CampaignRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Campaign>> GetAllCampaignsAsync()
        {
            var Campaigns = await _CampaignRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<Campaign>>(Campaigns);
        }

        public async Task<IEnumerable<Campaign>> GetCampaignsByKeywordAsync(string keyword)
        {
            var Campaigns = (await _CampaignRepository.GetAllAsync()).Where(c => c.Title.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));

            return _mapper.Map<IEnumerable<Campaign>>(Campaigns);
        }

        public async Task<Campaign> GetByIdAsync(Guid id)
        {
            var Campaign = await _CampaignRepository.GetByIdAsync(id);

            return _mapper.Map<Campaign>(Campaign);
        }

        public async Task<Campaign> CreateCampaignAsync(Campaign newCampaign)
        {
            var Campaign = await _CampaignRepository.CreateAsync(newCampaign);

            return _mapper.Map<Campaign>(Campaign);
        }

        public async Task<Campaign> UpdateCampaignAsync(Campaign newCampaign)
        {

            var Campaign = await _CampaignRepository.UpdateAsync(newCampaign);

            return _mapper.Map<Campaign>(Campaign);
        }

        public async Task<Campaign> DeactivateCampaignAsync(Guid id)
        {
            var existingCampaign = await _CampaignRepository.GetByIdAsync(id);

            //if (await IsActiveUserExistAsync(existingCampaign.Name))
            //{
            //    throw new Exception("User is in Campaign");
            //}

            existingCampaign.Status = StatusEnum.Inactive.ToString();

            var Campaign = await _CampaignRepository.UpdateAsync(existingCampaign);

            return _mapper.Map<Campaign>(Campaign);
        }

        public async Task<Campaign> ActivateCampaignAsync(Guid id)
        {
            var existingCampaign = await _CampaignRepository.GetByIdAsync(id);

            existingCampaign.Status = StatusEnum.Active.ToString();

            var Campaign = await _CampaignRepository.UpdateAsync(existingCampaign);

            return _mapper.Map<Campaign>(Campaign);
        }
    }
}
