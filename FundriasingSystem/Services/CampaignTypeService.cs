using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Enums;
using FundraisingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundraisingApp.Services
{
    public class CampaignTypeService
    {
        private readonly IRepository<CampaignType> _CampaignTypeRepository;
        private readonly IMapper _mapper;

        public CampaignTypeService(IRepository<CampaignType> CampaignTypeRepository,
            IMapper mapper)
        {
            _CampaignTypeRepository = CampaignTypeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CampaignType>> GetAllCampaignTypesAsync()
        {
            var CampaignTypes = await _CampaignTypeRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<CampaignType>>(CampaignTypes);
        }

        public async Task<CampaignType> GetByIdAsync(Guid id)
        {
            var CampaignType = await _CampaignTypeRepository.GetByIdAsync(id);

            return _mapper.Map<CampaignType>(CampaignType);
        }

        public async Task<CampaignType> CreateCampaignTypeAsync(CampaignType newCampaignType)
        {
            var CampaignType = await _CampaignTypeRepository.CreateAsync(newCampaignType);

            return _mapper.Map<CampaignType>(CampaignType);
        }

        public async Task<CampaignType> UpdateCampaignTypeAsync(CampaignType newCampaignType)
        {

            var CampaignType = await _CampaignTypeRepository.UpdateAsync(newCampaignType);

            return _mapper.Map<CampaignType>(CampaignType);
        }

        public async Task<CampaignType> DeactivateCampaignTypeAsync(Guid id)
        {
            var existingCampaignType = await _CampaignTypeRepository.GetByIdAsync(id);

            //if (await IsActiveUserExistAsync(existingCampaignType.Name))
            //{
            //    throw new Exception("User is in CampaignType");
            //}

            existingCampaignType.Status = StatusEnum.Inactive.ToString();

            var CampaignType = await _CampaignTypeRepository.UpdateAsync(existingCampaignType);

            return _mapper.Map<CampaignType>(CampaignType);
        }

        public async Task<CampaignType> ActivateCampaignTypeAsync(Guid id)
        {
            var existingCampaignType = await _CampaignTypeRepository.GetByIdAsync(id);

            existingCampaignType.Status = StatusEnum.Active.ToString();

            var CampaignType = await _CampaignTypeRepository.UpdateAsync(existingCampaignType);

            return _mapper.Map<CampaignType>(CampaignType);
        }
    }
}
